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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class GHN_CreateOrderRequest : CreateDeliveryOrderRequest, IRequest<BaseResult<CreateDeliveryOrderResponse>>
    {
    }

    public class CreateGhnOrderRequestHandler : IRequestHandler<GHN_CreateOrderRequest, BaseResult<CreateDeliveryOrderResponse>>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPartnerConfigService _partnerConfigService;
        private readonly IOrderCodeSequenceService _orderCodeSequenceService;
        private readonly IShopPartnerConfigRepository _shopPartnerConfigRepository;
        private readonly IGhnApiClient _ghnApiClient;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ILogger<CreateGhnOrderRequestHandler> _logger;
        private readonly IMediator _mediator;

        public CreateGhnOrderRequestHandler(
            IShopRepository shopRepository,
            IOrderRepository orderRepository,
            IOrderHistoryRepository orderHistoryRepository,
            IUnitOfWork unitOfWork,
            IPartnerConfigService partnerConfigService,
            IOrderCodeSequenceService orderCodeSequenceService,
            IShopPartnerConfigRepository shopPartnerConfigRepository,
            IGhnApiClient ghnApiClient,
            IMapper mapper,
            IAuthenticatedUserService authenticatedUserService,
            ILogger<CreateGhnOrderRequestHandler> logger,
            IMediator mediator)
        {
            _shopRepository = shopRepository;
            _orderRepository = orderRepository;
            _orderHistoryRepository = orderHistoryRepository;
            _unitOfWork = unitOfWork;
            _partnerConfigService = partnerConfigService;
            _orderCodeSequenceService = orderCodeSequenceService;
            _shopPartnerConfigRepository = shopPartnerConfigRepository;
            _ghnApiClient = ghnApiClient;
            _mapper = mapper;
            _authenticatedUserService = authenticatedUserService;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<BaseResult<CreateDeliveryOrderResponse>> Handle(GHN_CreateOrderRequest request, CancellationToken cancellationToken)
        {
            LogRequestData(request);

            var shop = await GetShopDetailsAsync(cancellationToken);
            if (shop == null || !shop.IsVerified)
            {
                return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.AccessDenied, "Cửa hàng chưa được kích hoạt, vui lòng liên hệ Admin"));
            }

            var deliveryConfig = await GetDeliveryConfigAsync(shop.Id, cancellationToken);
            if (deliveryConfig == null)
            {
                return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.NotFound, "Cấu hình vận chuyển không tồn tại."));
            }

            var apiConfig = new ApiConfig(deliveryConfig.PartnerConfig.ProdEnv, deliveryConfig.PartnerConfig.ApiKey);
            var response = await ProcessOrderAsync(shop, deliveryConfig, apiConfig, request);

            return response;
        }

        /// <summary>
        /// Handle send order to delivery partner
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="deliveryConfig"></param>
        /// <param name="apiConfig"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<BaseResult<CreateDeliveryOrderResponse>> ProcessOrderAsync(
            ShopQueryDto shop,
            DeliveryConfigDto deliveryConfig,
            ApiConfig apiConfig,
            GHN_CreateOrderRequest request)
        {
            // Process order with GHN partner
            if (deliveryConfig.PartnerConfig.DeliveryPartner == EnumDeliveryPartner.GHN)
            {
                // Override sender address
                if (shop.AllowUsePartnerShopAddress)
                {
                    int? _fromWardId = null;
                    string stringNumber = deliveryConfig.WardCode;
                    if (int.TryParse(stringNumber, out int __fromWardId))
                    {
                        _fromWardId = __fromWardId;
                    }
                    request.FromName = deliveryConfig.ShopName;
                    request.FromPhone = deliveryConfig.ClientPhone;
                    request.FromWardId = _fromWardId;
                    request.FromWardName = deliveryConfig.WardName;
                    request.FromDistrictId = int.Parse(deliveryConfig.DistrictId);
                    request.FromDistrictName = deliveryConfig.DistrictName;
                    request.FromProvinceId = int.Parse(deliveryConfig.ProvinceId);
                    request.FromProvinceName = deliveryConfig.ProvinceName;
                }

                var previewOrder = await _ghnApiClient.CreateDraftDeliveryOrderAsync(apiConfig, deliveryConfig.PartnerShopId, request);
                if (previewOrder.Code != 200)
                {
                    return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.Exception, $"{previewOrder.CodeMessageValue} {previewOrder.Message}"));
                }

                var (orderCode, orderId) = await SaveOrderAsync(request, shop, deliveryConfig.PartnerShopId);
                if (shop.AllowPublishOrder)
                {
                    var createOrder = await _ghnApiClient.CreateDeliveryOrderAsync(apiConfig, deliveryConfig.PartnerShopId, request);
                    if (createOrder.Code == 200)
                    {
                        await UpdateOrderAsync(createOrder.Data, orderId);
                        return BaseResult<CreateDeliveryOrderResponse>.Ok(createOrder.Data);
                    }
                }

                return BaseResult<CreateDeliveryOrderResponse>.Ok(previewOrder.Data);
            }

            return BaseResult<CreateDeliveryOrderResponse>.Ok(null);
        }

        private async Task<(string, Guid)> SaveOrderAsync(
            GHN_CreateOrderRequest request,
            ShopQueryDto shop,
            string partnerShopId)
        {
            var shopId = shop.Id;
            var uniqueShopCode = shop.UniqueCode;
            var allowPublishOrder = shop.AllowPublishOrder;
            var deliveryFeePlan = await CalculateDeliveryFeeAsync(request, shopId);

            var allowUsePartnerShopAddress = shop.AllowUsePartnerShopAddress;
            if (allowUsePartnerShopAddress)
            {

            }

            var order = CreateOrderEntity(
                request,
                shop,
                deliveryFeePlan,
                partnerShopId);
            order.OrrverideDeliveryFee(order.DeliveryFee);
            order.GenerateOrderCode(await _orderCodeSequenceService.GenerateOrderCodeAsync(shopId), uniqueShopCode);
            await _orderRepository.AddAsync(order);
            await _orderHistoryRepository.AddAsync(new OrderStatusHistory
            {
                OrderId = order.Id,
                ChangedBy = shopId.ToString(),
                Status = allowPublishOrder ? OrderStatus.READY_TO_PICK : OrderStatus.WAITING_CONFIRM,
                Notes = "Order created via API"
            });

            await _unitOfWork.SaveChangesAsync();

            return (order.ClientOrderCode, order.Id);
        }

        private Order CreateOrderEntity(
            GHN_CreateOrderRequest request,
            ShopQueryDto shop,
            int deliveryFeePlan,
            string partnerShopId)
        {
            var shopId = shop.Id;
            var uniqueShopCode = shop.UniqueCode;
            var allowPublishOrder = shop.AllowPublishOrder;
            var allowUsePartnerShopAddress = shop.AllowUsePartnerShopAddress;

            return new Order
            {
                PartnerShopId = partnerShopId,
                ShopId = shopId,
                UniqueCode = uniqueShopCode,
                IsPublished = allowPublishOrder ? true : false,
                DeliveryPartner = EnumSupplierConstants.GHN,
                DeliveryFee = deliveryFeePlan,
                PublishDate = allowPublishOrder ? DateTime.UtcNow : null,
                CurrentStatus = allowPublishOrder ? OrderStatus.READY_TO_PICK : OrderStatus.WAITING_CONFIRM,
                Note = request.Note,
                ReturnPhone = request.ReturnPhone,
                ReturnAddress = request.ReturnAddress,
                ReturnDistrictId = request.ReturnDistrictName,
                ReturnWardCode = request.ReturnWardName,
                ReturnDistrictName = request.ReturnDistrictName,
                ReturnWardName = request.ReturnWardName,

                ClientOrderCode = uniqueShopCode,
                PaymentTypeId = request.PaymentTypeId,
                RequiredNote = request.RequiredNote,

                FromName = request.FromName,
                FromPhone = request.FromPhone,
                FromAddress = request.FromAddress,
                FromWardId = request.FromWardId,
                FromWardName = request.FromWardName,
                FromDistrictId = request.FromDistrictId,
                FromDistrictName = request.FromDistrictName,
                FromProvinceId = request.FromProvinceId,
                FromProvinceName = request.FromProvinceName,

                ToName = request.ToName,
                ToPhone = request.ToPhone,
                ToAddress = request.ToAddress,
                ToWardId = request.ToWardId,
                ToWardName = request.ToWardName,
                ToDistrictId = request.ToDistrictId,
                ToDistrictName = request.ToDistrictName,
                ToProvinceId = request.ToProvinceId,
                ToProvinceName = request.ToProvinceName,

                CodAmount = request.CodAmount,
                Content = request.Content,
                PickStationId = request.PickStationId,
                DeliverStationId = request.DeliverStationId,
                InsuranceValue = request.InsuranceValue,
                ServiceId = request.ServiceId,
                ServiceTypeId = request.ServiceTypeId,
                Coupon = request.Coupon,
                PickShift = request.PickShift,

                Weight = request.Weight,
                Length = request.Length,
                Width = request.Width,
                Height = request.Height,
                Items = request.Items.Select(i => new OrderItem
                {
                    Name = i.Name,
                    Code = i.Code,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Length = i.Length,
                    Width = i.Width,
                    Height = i.Height,
                    Weight = i.Weight,
                }).ToList(),
            };
        }

        private async Task UpdateOrderAsync(CreateDeliveryOrderResponse response, Guid orderId)
        {
            var entity = await _orderRepository.Where(o => o.Id == orderId).FirstOrDefaultAsync();
            if (entity == null) return;

            // Duplicated: update order from parter
            _unitOfWork.Orders.Modify(entity);
            entity.Publish();
            entity.PrivateUpdateFromPartner(
                private_order_code: response.order_code,
                private_sort_code: response.sort_code,
                private_trans_type: response.trans_type,
                private_total_fee: response.total_fee,
                private_expected_delivery_time: response.expected_delivery_time,
                private_operation_partner: response.operation_partner
                );

            await _orderHistoryRepository.AddAsync(new OrderStatusHistory
            {
                OrderId = entity.Id,
                ChangedBy = "CreateGhnOrderRequest",
                Status = OrderStatus.READY_TO_PICK,
                Notes = "Order send to GHN success"
            });

            await _unitOfWork.SaveChangesAsync();

            await _mediator.Send(new GHN_SyncOrderRequest
            {
                PartnerOrderCode = entity.private_order_code
            });
        }

        private async Task<int> CalculateDeliveryFeeAsync(GHN_CreateOrderRequest request, Guid shopId)
        {
            string deliveryPartner = EnumSupplierConstants.GHN;
            var calculateWeight = request.Length * request.Width * request.Height;
            var result = await _unitOfWork.ShopPricePlanes
                .Where(i => i.ShopId == shopId && i.Supplier == deliveryPartner && i.ConvertedWeight > calculateWeight)
                .OrderBy(i => i.ConvertedWeight)
                .Select(i => i.OfficialPrice)
                .FirstOrDefaultAsync();

            return (int)result;
        }

        private void LogRequestData(GHN_CreateOrderRequest request)
        {
            _logger.LogInformation("{Handler} - Data: {Data}", nameof(CreateGhnOrderRequestHandler), JsonConvert.SerializeObject(request));
        }

        internal class ShopQueryDto
        {
            public Guid Id { get; set; }
            public string UniqueCode { get; set; }
            public bool AllowPublishOrder { get; set; }
            public bool AllowUsePartnerShopAddress { get; set; }
            public bool IsVerified { get; set; }

        }
        private async Task<ShopQueryDto> GetShopDetailsAsync(CancellationToken cancellationToken)
        {
            var userId = _authenticatedUserService.UId;
            return await _shopRepository
                .Where(i => i.AccountId == userId)
                .Select(i => new ShopQueryDto
                {
                    Id = i.Id,
                    UniqueCode = i.UniqueCode,
                    AllowPublishOrder = i.AllowPublishOrder,
                    AllowUsePartnerShopAddress = i.AllowUsePartnerShopAddress,
                    IsVerified = i.IsVerified
                })
                .FirstOrDefaultAsync(cancellationToken);
        }


        internal class DeliveryConfigDto
        {
            public Guid ShopId { get; set; }
            public Guid PartnerConfigId { get; set; }
            public string PartnerShopId { get; set; }

            public string ClientPhone { get; set; }
            public string? Address { get; set; }
            public string? WardName { get; set; }
            public string? WardCode { get; set; }
            public string? DistrictName { get; set; }
            public string? DistrictId { get; set; }
            public string? ProvinceName { get; set; }
            public string? ProvinceId { get; set; }
            public string ShopName { get; set; }

            public PartnerConfigDto PartnerConfig { get; set; }
        }

        internal class PartnerConfigDto
        {
            public string ApiKey { get; set; }
            public string ProdEnv { get; set; }
            public EnumDeliveryPartner DeliveryPartner { get; set; }
        }

        private async Task<DeliveryConfigDto> GetDeliveryConfigAsync(Guid shopId, CancellationToken cancellationToken)
        {
            var result = await _shopPartnerConfigRepository
               .Where(i => i.ShopId == shopId && i.PartnerConfig.IsActivated)
               .Select(i => new DeliveryConfigDto
               {
                   ShopId = i.ShopId,
                   PartnerConfigId = i.PartnerConfigId,
                   PartnerShopId = i.PartnerShopId,
                   ClientPhone = i.ClientPhone,
                   Address = i.Address,
                   WardCode = i.WardCode,
                   WardName = i.WardName,
                   DistrictId = i.DistrictId,
                   DistrictName = i.DistrictName,
                   ProvinceId = i.ProvinceId,
                   ProvinceName = i.ProvinceName,
                   ShopName = i.ShopName,
                   PartnerConfig = new PartnerConfigDto
                   {
                       ApiKey = i.PartnerConfig.ApiKey,
                       ProdEnv = i.PartnerConfig.ProdEnv,
                       DeliveryPartner = i.PartnerConfig.DeliveryPartner
                   }
               })
               .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
