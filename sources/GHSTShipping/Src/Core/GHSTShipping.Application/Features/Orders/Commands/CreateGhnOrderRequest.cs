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
    public class CreateGhnOrderRequest : CreateDeliveryOrderRequest, IRequest<BaseResult<CreateDeliveryOrderResponse>> { }

    public class CreateGhnOrderRequestHandler : IRequestHandler<CreateGhnOrderRequest, BaseResult<CreateDeliveryOrderResponse>>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPartnerConfigService _partnerConfigService;
        private readonly IOrderCodeSequenceService _orderCodeSequenceService;
        private readonly IShopPartnerConfigRepository _shopPartnerConfigRepository;
        private readonly IGhnApiClient _ghnApiClient;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ILogger<CreateGhnOrderRequestHandler> _logger;

        public CreateGhnOrderRequestHandler(
            IShopRepository shopRepository,
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IPartnerConfigService partnerConfigService,
            IOrderCodeSequenceService orderCodeSequenceService,
            IShopPartnerConfigRepository shopPartnerConfigRepository,
            IGhnApiClient ghnApiClient,
            IMapper mapper,
            IAuthenticatedUserService authenticatedUserService,
            ILogger<CreateGhnOrderRequestHandler> logger)
        {
            _shopRepository = shopRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _partnerConfigService = partnerConfigService;
            _orderCodeSequenceService = orderCodeSequenceService;
            _shopPartnerConfigRepository = shopPartnerConfigRepository;
            _ghnApiClient = ghnApiClient;
            _mapper = mapper;
            _authenticatedUserService = authenticatedUserService;
            _logger = logger;
        }

        public async Task<BaseResult<CreateDeliveryOrderResponse>> Handle(CreateGhnOrderRequest request, CancellationToken cancellationToken)
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

        private async Task<BaseResult<CreateDeliveryOrderResponse>> ProcessOrderAsync(
            ShopQueryDto shop,
            DeliveryConfigDto deliveryConfig,
            ApiConfig apiConfig,
            CreateGhnOrderRequest request)
        {
            var previewOrder = await _ghnApiClient.CreateDraftDeliveryOrderAsync(apiConfig, deliveryConfig.PartnerShopId, request);
            if (previewOrder.Code != 200)
            {
                return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.Exception, $"{previewOrder.CodeMessageValue} {previewOrder.Message}"));
            }

            var (orderCode, orderId) = await SaveOrderAsync(request, shop.Id, shop.UniqueCode, shop.AllowPublishOrder);

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

        private async Task<(string, Guid)> SaveOrderAsync(
            CreateGhnOrderRequest request,
            Guid shopId,
            string uniqueShopCode,
            bool allowPublishOrder)
        {
            var deliveryFeePlan = await CalculateDeliveryFeeAsync(request, shopId);
            var order = CreateOrderEntity(request, shopId, uniqueShopCode, allowPublishOrder, deliveryFeePlan);
            order.GenerateOrderCode(await _orderCodeSequenceService.GenerateOrderCodeAsync(shopId), uniqueShopCode);

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return (order.ClientOrderCode, order.Id);
        }

        private Order CreateOrderEntity(CreateGhnOrderRequest request, Guid shopId, string uniqueShopCode, bool allowPublishOrder, int deliveryFeePlan)
        {
            return new Order
            {
                Note = request.Note,
                PaymentTypeId = request.PaymentTypeId,
                RequiredNote = request.RequiredNote,
                FromName = request.FromName,
                FromPhone = request.FromPhone,
                ToName = request.ToName,
                ToPhone = request.ToPhone,
                ToAddress = request.ToAddress,
                Weight = request.Weight,
                Length = request.Length,
                Width = request.Width,
                Height = request.Height,
                Items = request.Items.Select(i => new OrderItem
                {
                    Name = i.Name,
                    Code = i.Code,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
                CodAmount = request.CodAmount,
                ShopId = shopId,
                DeliveryPartner = EnumSupplierConstants.GHN,
                DeliveryFee = deliveryFeePlan,
                PublishDate = allowPublishOrder ? DateTime.UtcNow : null,
            };
        }

        private async Task UpdateOrderAsync(CreateDeliveryOrderResponse response, Guid orderId)
        {
            var entity = await _orderRepository.Where(o => o.Id == orderId).FirstOrDefaultAsync();
            if (entity == null) return;

            _unitOfWork.Orders.Modify(entity);
            entity.private_order_code = response.order_code;
            entity.private_sort_code = response.sort_code;
            entity.private_trans_type = response.trans_type;
            entity.private_total_fee = response.total_fee;
            entity.private_expected_delivery_time = response.expected_delivery_time;
            entity.private_operation_partner = response.operation_partner;

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<int> CalculateDeliveryFeeAsync(CreateGhnOrderRequest request, Guid shopId)
        {
            string deliveryPartner = EnumSupplierConstants.GHN;
            var result = await _unitOfWork.ShopPricePlanes
                .Where(i => i.ShopId == shopId && i.Supplier == deliveryPartner && i.Capacity > request.Weight)
                .OrderBy(i => i.Capacity)
                .Select(i => i.OfficialPrice)
                .FirstOrDefaultAsync();

            return (int)result;
        }

        private void LogRequestData(CreateGhnOrderRequest request)
        {
            _logger.LogInformation("{Handler} - Data: {Data}", nameof(CreateGhnOrderRequestHandler), JsonConvert.SerializeObject(request));
        }

        internal class ShopQueryDto
        {
            public Guid Id { get; set; }
            public string UniqueCode { get; set; }
            public bool AllowPublishOrder { get; set; }
            public bool IsVerified { get; set; }
        }
        private async Task<ShopQueryDto> GetShopDetailsAsync(CancellationToken cancellationToken)
        {
            var userId = _authenticatedUserService.UId;
            return await _shopRepository
                .Where(i => i.AccountId == userId)
                .Select(i => new ShopQueryDto { Id = i.Id, UniqueCode = i.UniqueCode, AllowPublishOrder = i.AllowPublishOrder, IsVerified = i.IsVerified })
                .FirstOrDefaultAsync(cancellationToken);
        }


        internal class DeliveryConfigDto
        {
            public Guid ShopId { get; set; }
            public Guid PartnerConfigId { get; set; }
            public string PartnerShopId { get; set; }
            public string ClientPhone { get; set; }
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
            return await _shopPartnerConfigRepository
                .Where(i => i.ShopId == shopId)
                .Select(i => new DeliveryConfigDto
                {
                    ShopId = i.ShopId,
                    PartnerConfigId = i.PartnerConfigId,
                    PartnerShopId = i.PartnerShopId,
                    ClientPhone = i.ClientPhone,
                    PartnerConfig = new PartnerConfigDto
                    {
                        ApiKey = i.PartnerConfig.ApiKey,
                        ProdEnv = i.PartnerConfig.ProdEnv,
                        DeliveryPartner = i.PartnerConfig.DeliveryPartner
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
