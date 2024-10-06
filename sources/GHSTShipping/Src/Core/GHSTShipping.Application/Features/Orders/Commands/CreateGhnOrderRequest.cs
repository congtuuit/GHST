﻿using AutoMapper;
using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class CreateGhnOrderRequest : CreateDeliveryOrderRequest, IRequest<BaseResult<CreateDeliveryOrderResponse>>
    {
        public int ShopId { get; set; }
    }

    public class CreateGhnOrderRequestHandler : IRequestHandler<CreateGhnOrderRequest, BaseResult<CreateDeliveryOrderResponse>>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPartnerConfigService _partnerConfigService;
        private readonly IGhnApiClient _ghnApiClient;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ILogger<CreateGhnOrderRequestHandler> _logger;

        public CreateGhnOrderRequestHandler(
            IShopRepository shopRepository,
            IOrderRepository genericRepository,
            IUnitOfWork unitOfWork,
            IPartnerConfigService partnerConfigService,
            IGhnApiClient ghnApiClient,
            IMapper mapper,
            IAuthenticatedUserService authenticatedUserService,
            ILogger<CreateGhnOrderRequestHandler> logger)
        {
            _shopRepository = shopRepository;
            _orderRepository = genericRepository;
            _unitOfWork = unitOfWork;
            _partnerConfigService = partnerConfigService;
            _mapper = mapper;
            _ghnApiClient = ghnApiClient;
            _authenticatedUserService = authenticatedUserService;
            _logger = logger;
        }

        public async Task<BaseResult<CreateDeliveryOrderResponse>> Handle(CreateGhnOrderRequest request, CancellationToken cancellationToken)
        {
            // Add log trace for order
            var logData = JsonConvert.SerializeObject(request);
            _logger.LogInformation(nameof(CreateGhnOrderRequestHandler) + " {data}", logData);

            // TODO get new fee from system config
            int weight = request.weight;
            request.ShopId = 5356247; // this shop ID from GHN partner

            var userId = _authenticatedUserService.UId;
            var shop = await _shopRepository.Where(i => i.AccountId == userId).Select(i => new
            {
                ShopId = i.Id,
                i.UniqueCode,
                i.AllowPublishOrder,
                i.IsVerified,
            })
            .FirstOrDefaultAsync();

            if (!shop.IsVerified) { 
                    return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.AccessDenied, "Cửa hàng chưa được kích hoạt, vui lòng liên hệ Admin"));
            }


            // client order code generated by sql server
            var (orderCode, orderId) = await HandleSaveOrderAsync(request, shop.ShopId, shop.AllowPublishOrder);

            // Get API config to send request to GHN
            var partnerConfig = await _partnerConfigService.GetPartnerConfigAsync(Domain.Enums.EnumDeliveryPartner.GHN);
            var apiConfig = new ApiConfig(partnerConfig.ProdEnv, partnerConfig.ApiKey);

            // Mapping order to GHN
            var deliveryOrderRequest = _mapper.Map<CreateDeliveryOrderRequest>(request);
            deliveryOrderRequest.client_order_code = orderCode;

            if (shop.AllowPublishOrder)
            {
                var apiResult = await _ghnApiClient.CreateDeliveryOrderAsync(apiConfig, request.ShopId, deliveryOrderRequest);
                if (apiResult.Code == 200)
                {
                    await UpdateOrderAsync(apiResult.Data, orderId);

                    return BaseResult<CreateDeliveryOrderResponse>.Ok(apiResult.Data);
                }
                else
                {
                    return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.Exception, apiResult.Message));
                }
            }
            else
            {
                // Send order to GHN
                var apiResult = await _ghnApiClient.CreateDraftDeliveryOrderAsync(apiConfig, request.ShopId, deliveryOrderRequest);
                if (apiResult.Code == 200)
                {
                    await UpdateOrderAsync(apiResult.Data, orderId);

                    return BaseResult<CreateDeliveryOrderResponse>.Ok(apiResult.Data);
                }
                else
                {
                    return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.Exception, apiResult.Message));
                }
            }
        }

        private async Task<(string, Guid)> HandleSaveOrderAsync(CreateGhnOrderRequest request, Guid shopId, bool allowPublishOrder)
        {
            var deliveryFee = await CalcDeliveryFeeAsync(request, shopId);

            // TODO save to DB
            var order = new Order()
            {
                Note = request.note,
                ReturnAddress = request.return_address,
                ReturnDistrictId = request.return_district_id,
                ReturnWardCode = request.return_ward_code,
                PaymentTypeId = request.payment_type_id,
                RequiredNote = request.required_note,
                FromName = request.from_name,
                FromPhone = request.from_phone,
                FromWardName = request.from_ward_name,
                FromDistrictName = request.from_district_name,
                FromProvinceName = request.from_province_name,
                ToName = request.to_name,
                ToPhone = request.to_phone,
                ToAddress = request.to_address,
                ToWardCode = request.to_ward_code,
                ToDistrictId = request.to_district_id,
                Weight = request.weight,
                Length = request.length,
                Width = request.width,
                Height = request.height,
                Items = request.items.Select(i => new OrderItem
                {
                    Name = i.name,
                    Code = i.code,
                    Quantity = i.quantity,
                    Price = i.price,
                    Length = i.length,
                    Width = i.width,
                    Height = i.height,
                }).ToList(),
                CodAmount = request.cod_amount,
                Content = request.content,
                PickStationId = request.pick_station_id,
                DeliverStationId = request.deliver_station_id,
                InsuranceValue = request.insurance_value,
                ServiceId = request.service_id,
                ServiceTypeId = request.service_type_id,
                Coupon = request.coupon,
                PickShift = request.pick_shift,

                ShopId = shopId,
                DeliveryPartner = EnumSupplierConstants.GHN,
                DeliveryFee = deliveryFee,
                PublishDate = allowPublishOrder ? DateTime.UtcNow : null,
            };

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            var orderIds = order.Id.ToString();
            var results = await _orderRepository.UpdateOrderCodeSequenceAsync(orderIds, shopId);
            if (results.Any())
            {
                var result = results.FirstOrDefault();
                return (result.OrderCodeSequence, order.Id);
            }
            else
            {
                return (string.Empty, Guid.Empty);
            }
        }

        private async Task<int> CalcDeliveryFeeAsync(CreateGhnOrderRequest request, Guid shopId)
        {
            string deliveryPartner = EnumSupplierConstants.GHN;
            var maxPrice = await _unitOfWork.ShopPricePlanes
                .Where(i => i.ShopId == shopId && i.Supplier == deliveryPartner && i.Capacity > request.weight)
                .OrderBy(i => i.Capacity)
                .Select(i => i.OfficialPrice)
                .FirstOrDefaultAsync();

            return (int)maxPrice;
        }

        private async Task UpdateOrderAsync(CreateDeliveryOrderResponse response, Guid orderId)
        {
            var entity = await _orderRepository.Where(o => o.Id == orderId)
                .Select(o => new Order
                {
                    Id = o.Id
                })
                .FirstOrDefaultAsync();

            if (entity != null)
            {
                _unitOfWork.Orders.Modify(entity);
                entity.private_order_code = response.order_code;
                entity.private_sort_code = response.sort_code;
                entity.private_trans_type = response.trans_type;
                entity.private_total_fee = response.total_fee;
                entity.private_expected_delivery_time = response.expected_delivery_time;
                entity.private_operation_partner = response.operation_partner;

                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
