using AutoMapper;
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
        public string ShopId { get; set; }
    }

    public class CreateGhnOrderRequestHandler : IRequestHandler<CreateGhnOrderRequest, BaseResult<CreateDeliveryOrderResponse>>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPartnerConfigService _partnerConfigService;
        private readonly IOrderCodeSequenceService _orderCodeSequenceService;
        private readonly IPartnerConfigRepository _partnerConfigRepository;
        private readonly IShopPartnerConfigRepository _shopPartnerConfigRepository;

        private readonly IMediator _mediator;
        private readonly IGhnApiClient _ghnApiClient;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ILogger<CreateGhnOrderRequestHandler> _logger;

        public CreateGhnOrderRequestHandler(
            IShopRepository shopRepository,
            IOrderRepository genericRepository,
            IUnitOfWork unitOfWork,
            IPartnerConfigService partnerConfigService,
            IOrderCodeSequenceService orderCodeSequenceService,
            IPartnerConfigRepository partnerConfigRepository,
            IShopPartnerConfigRepository shopPartnerConfigRepository,
            IMediator mediator,
            IGhnApiClient ghnApiClient,
            IMapper mapper,
            IAuthenticatedUserService authenticatedUserService,
            ILogger<CreateGhnOrderRequestHandler> logger)
        {
            _shopRepository = shopRepository;
            _orderRepository = genericRepository;
            _unitOfWork = unitOfWork;
            _partnerConfigService = partnerConfigService;
            _orderCodeSequenceService = orderCodeSequenceService;
            _partnerConfigRepository = partnerConfigRepository;
            _shopPartnerConfigRepository = shopPartnerConfigRepository;
            _mediator = mediator;
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

            var userId = _authenticatedUserService.UId;
            var shop = await _shopRepository.Where(i => i.AccountId == userId).Select(i => new
            {
                ShopId = i.Id,
                i.UniqueCode,
                i.AllowPublishOrder,
                i.IsVerified,
            })
            .FirstOrDefaultAsync(cancellationToken);

            if (!shop.IsVerified)
            {
                return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.AccessDenied, "Cửa hàng chưa được kích hoạt, vui lòng liên hệ Admin"));
            }


            /*// Get delivery config for the store
            var orderMetaData = await _mediator.Send(new GetOrderMetadataRequest());
            if (!orderMetaData.Success)
            {
                // KHÔNG CÓ CẤU HÌNH KẾT NỐI
                return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.AccessDenied, "Cửa hàng chưa được kích hoạt, vui lòng liên hệ Admin"));
            }

            var deliveryConfigs = orderMetaData.Data.DeliveryConfigs;*/

            // Sender address is the shop in delivery partner connected to the store
            //await OverrideSenderAddressAsync(request, deliveryConfigs);

            // Get delivery config
            var deliveryConfig = await _shopPartnerConfigRepository.Where(i => i.ShopId == shop.ShopId).Select(i => new
            {
                i.ShopId,
                i.PartnerConfigId,
                i.PartnerShopId,
                i.ClientPhone,
                PartnerConfig = new
                {
                    i.PartnerConfig.ApiKey,
                    i.PartnerConfig.ProdEnv,
                    i.PartnerConfig.DeliveryPartner
                }
            }).FirstOrDefaultAsync(cancellationToken);

            var apiConfig = new ApiConfig(deliveryConfig.PartnerConfig.ProdEnv, deliveryConfig.PartnerConfig.ApiKey);
            request.ShopId = deliveryConfig.PartnerShopId;


            // client order code generated by sql server
            var (orderCode, orderId) = await HandleSaveOrderAsync(request, shop.ShopId, shop.UniqueCode, shop.AllowPublishOrder);

            // Mapping order to GHN
            var deliveryOrderRequest = _mapper.Map<CreateDeliveryOrderRequest>(request);
            deliveryOrderRequest.client_order_code = orderCode;

            

            if (shop.AllowPublishOrder)
            {
                if (deliveryConfig.PartnerConfig.DeliveryPartner == EnumDeliveryPartner.GHN)
                {

                    var apiResult = await _ghnApiClient.CreateDeliveryOrderAsync(apiConfig, request.ShopId, deliveryOrderRequest);
                    if (apiResult.Code == 200)
                    {
                        await UpdateOrderAsync(apiResult.Data, orderId);

                        return BaseResult<CreateDeliveryOrderResponse>.Ok(apiResult.Data);
                    }
                    else
                    {
                        return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.Exception, apiResult.CodeMessageValue));
                    }
                }

                // TODO handle for another partner

            }
            else
            {
                if (deliveryConfig.PartnerConfig.DeliveryPartner == EnumDeliveryPartner.GHN)
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
                        return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.Exception, apiResult.CodeMessageValue));
                    }
                }

                // TODO handle for another partner
            }

            return BaseResult<CreateDeliveryOrderResponse>.Ok(new CreateDeliveryOrderResponse());
        }

        private async Task<(string, Guid)> HandleSaveOrderAsync(
            CreateGhnOrderRequest request,
            Guid shopId,
            string uniqueShopCode,
            bool allowPublishOrder)
        {
            var deliveryFee = await CalcDeliveryFeeAsync(request, shopId);

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

            long orderCode = await _orderCodeSequenceService.GenerateOrderCodeAsync(shopId);

            order.GenerateOrderCode(orderCode, uniqueShopCode);

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return (order.ClientOrderCode, order.Id);
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

        /*private async Task<ApiConfig> OverrideSenderAddressAsync(CreateGhnOrderRequest request, List<DeliveryConfigDto> deliveryConfigs)
        {
            var config = deliveryConfigs.FirstOrDefault();
            if (config == null)
            {
                // TODO throw error
            }
            else
            {
                if (config.DeliveryPartner == EnumDeliveryPartner.GHN)
                {
                    var deliveryConfig = await _partnerConfigRepository.Where(i => i.Id == config.DeliveryConfigId)
                        .Select(i => new
                        {
                            i.ProdEnv,
                            i.ApiKey,
                        })
                        .FirstOrDefaultAsync();

                    var apiConfig = new ApiConfig(deliveryConfig.ProdEnv, deliveryConfig.ApiKey);

                    //var shopConnected = config.Shops.First();

                    // TODO NEED DISCUS
                    *//*request.from_address = shopConnected.Address;
                    request.from_phone = shopConnected.Phone;
                    request.from_ward_name = shopConnected.WardCode;*//*

                    return apiConfig;
                }
            }

            return new ApiConfig(string.Empty, string.Empty);
        }*/
    }
}
