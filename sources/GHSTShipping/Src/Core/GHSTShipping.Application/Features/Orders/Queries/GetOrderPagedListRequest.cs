using AutoMapper;
using AutoMapper.QueryableExtensions;
using Delivery.GHN;
using Delivery.GHN.Constants;
using Delivery.GHN.Models;
using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.Extensions;
using GHSTShipping.Application.Features.Configs.Queries;
using GHSTShipping.Application.Features.Orders.Commands;
using GHSTShipping.Application.Helpers;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Parameters;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Queries
{
    public class GetOrderPagedListRequest : PaginationRequestParameter, IRequest<BaseResult<PaginationResponseDto<OrderDto>>>
    {
        #region GHN filter
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PaymentTypeId { get; set; }
        public bool? IsPrint { get; set; }
        public bool? IsCodFailedCollected { get; set; }
        public bool? IsDocumentPod { get; set; }
        public OrderGroupStatus GroupStatus { get; set; }
        public string Status { get; set; }

        /// <summary>
        /// The partner order code like codeA,codeB,...
        /// </summary>
        public string OrderCode { get; set; }

        #endregion

        public string DeliveryPartner { get; set; }

        public Guid? ShopId { get; set; }
    }

    public class GetOrderPagedListRequestHandler(
    IMediator mediator,
    IGhnApiClient ghnApiClient,
    IUnitOfWork unitOfWork,
    IAuthenticatedUserService authenticatedUser,
    IShopRepository shopRepository,
    IPartnerConfigService partnerConfigService,
    IMapper mapper,
    IServiceScopeFactory serviceScopeFactory,
    MapperConfiguration mapperConfiguration
) : IRequestHandler<GetOrderPagedListRequest, BaseResult<PaginationResponseDto<OrderDto>>>
    {
        public async Task<BaseResult<PaginationResponseDto<OrderDto>>> Handle(GetOrderPagedListRequest request, CancellationToken cancellationToken)
        {
            var (shopId, shopUniqueCode) = await GetShopInfoAsync(request.ShopId, authenticatedUser, shopRepository, cancellationToken);

            /// Nếu không có giá trị shopId và người dùng không phải là admin, trả về kết quả rỗng
            /// - !shopId.HasValue: Không xác định được shopId từ yêu cầu hoặc tài khoản người dùng
            /// - !authenticatedUser.IsAdmin: Người dùng hiện tại không có quyền admin
            /// => Điều này có nghĩa là người dùng không có quyền truy cập bất kỳ shop nào hoặc đơn hàng nào
            if (!shopId.HasValue && !authenticatedUser.IsAdmin) return EmptyResult(request);

            var skipCount = (request.PageNumber - 1) * request.PageSize;
            IQueryable<Order> query = unitOfWork.Orders.All().AsNoTracking();

            if (IsLocalQuery(request.GroupStatus))
            {
                query = ApplyFilters(query, request, shopId);

                var pagingResult = await query
                    .OrderByDescending(i => i.Created)
                    .ProjectTo<OrderDto>(mapperConfiguration)
                    .ToPaginationAsync(request.PageNumber, request.PageSize, cancellationToken);

                var deliveryPricePlaneIds = pagingResult.Data
                    .Where(i => i.DeliveryPricePlaneId.HasValue)
                    .Select(i => i.DeliveryPricePlaneId.Value)
                    .ToList();

                var deliveryPricePlanes = await mediator.Send(new GetShopDeliveryPricePlanesRequest
                {
                    Ids = deliveryPricePlaneIds
                });

                return MapPaginationResult(pagingResult, skipCount, authenticatedUser.IsAdmin, deliveryPricePlanes.Data);
            }
            else if (await ShouldSyncFromGHN(request, shopId))
            {
                return await SyncOrdersFromGHNAsync(request, shopId.Value, shopUniqueCode, skipCount, cancellationToken);
            }

            return BaseResult<PaginationResponseDto<OrderDto>>.Ok(null);
        }

        private static async Task<(Guid? ShopId, string UniqueCode)> GetShopInfoAsync(
            Guid? requestShopId,
            IAuthenticatedUserService authenticatedUser,
            IShopRepository shopRepository,
            CancellationToken cancellationToken)
        {
            // Xác định điều kiện truy vấn shop
            var query = authenticatedUser.IsAdmin && requestShopId.HasValue
                ? shopRepository.Where(i => i.Id == requestShopId.Value)
                : shopRepository.Where(i => i.AccountId == authenticatedUser.UId);

            // Thực hiện truy vấn và lấy kết quả
            var shop = await query
                .Select(i => new { i.Id, i.UniqueCode })
                .FirstOrDefaultAsync(cancellationToken);

            // Trả về kết quả
            return (shop?.Id, shop?.UniqueCode ?? string.Empty);
        }

        private IQueryable<Order> ApplyFilters(IQueryable<Order> query, GetOrderPagedListRequest request, Guid? shopId)
        {
            if (shopId.HasValue)
            {
                query = query.Where(i => i.ShopId == shopId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.OrderCode))
            {
                query = query.Where(o => o.ClientOrderCode.Contains(request.OrderCode) || request.OrderCode.Contains(o.ClientOrderCode));
            }

            if (!string.IsNullOrWhiteSpace(request.DeliveryPartner))
            {
                query = query.Where(o => o.DeliveryPartner.Contains(request.DeliveryPartner));
            }

            if (request.GroupStatus == OrderGroupStatus.Nhap)
            {
                query = query.Where(o => o.IsPublished == false && o.CurrentStatus == OrderStatus.DRAFT);
            }
            else if (request.GroupStatus == OrderGroupStatus.ChoXacNhan)
            {
                query = query.Where(o => o.IsPublished == false && o.CurrentStatus == OrderStatus.WAITING_CONFIRM);
            }

            return query;
        }

        private async Task<bool> ShouldSyncFromGHN(GetOrderPagedListRequest request, Guid? shopId)
        {
            if (!shopId.HasValue) return false;

            var shopConfig = await partnerConfigService.GetShopConfigsAsync(shopId.Value);
            var deliveryPartner = shopConfig.FirstOrDefault()?.DeliveryPartner;

            return request.DeliveryPartner == EnumDeliveryPartner.GHN.GetCode() || deliveryPartner == EnumDeliveryPartner.GHN;
        }

        private async Task<BaseResult<PaginationResponseDto<OrderDto>>> SyncOrdersFromGHNAsync(
            GetOrderPagedListRequest request, Guid shopId, string shopUniqueCode, int skipCount, CancellationToken cancellationToken)
        {
            var apiConfigs = await OrderUtils.GetApiConfigsByShopIdAsync(unitOfWork, shopId, EnumDeliveryPartner.GHN);
            if (apiConfigs.Count == 0)
            {
                throw new Exception("Partner config not found");
            }

            List<Order> syncOrders = [];
            List<OrderItem> syncOrderItems = [];
            foreach (var apiConfig in apiConfigs)
            {
                var (entityOrders, entityOrderItems) = await GHN_HandleSearchOrdersAsync(apiConfig, shopId, shopUniqueCode, request);
                syncOrders.AddRange(entityOrders);
                syncOrderItems.AddRange(entityOrderItems);
            }

            if (syncOrders.Count > 0)
            {
                var orderIdsSaved = await GHN_SyncOrderRequestHandler.BatchSaveAsync(unitOfWork, syncOrders, syncOrderItems);
                var mappedOrders = await unitOfWork.Orders.Where(i => orderIdsSaved.Contains(i.Id))
                    .ProjectTo<OrderDto>(mapperConfiguration)
                    .ToPaginationAsync(request.PageNumber, request.PageSize, cancellationToken: cancellationToken);

                var deliveryPricePlaneIds = mappedOrders.Data
                   .Where(i => i.DeliveryPricePlaneId.HasValue)
                   .Select(i => i.DeliveryPricePlaneId.Value)
                   .ToList();

                var deliveryPricePlanes = await mediator.Send(new GetShopDeliveryPricePlanesRequest
                {
                    Ids = deliveryPricePlaneIds
                });

                return MapPaginationResult(mappedOrders, skipCount, authenticatedUser.IsAdmin, deliveryPricePlanes.Data);
            }

            return BaseResult<PaginationResponseDto<OrderDto>>.Ok(null);
        }

        private async Task<(List<Order>, List<OrderItem>)> GHN_HandleSearchOrdersAsync(ApiConfig apiConfig, Guid shopId, string shopUniqueCode, GetOrderPagedListRequest request)
        {
            var searchParams = new Delivery.GHN.Models.ShippingOrderSearchRequest
            {
                Status = request.GroupStatus.GetDetails(),
                ShopId = int.Parse(apiConfig.ShopId),
                Limit = request.PageSize,
                Offset = request.PageNumber > 0 ? (request.PageNumber - 1) * request.PageSize : 0,
            };

            if (request.PaymentTypeId.HasValue)
            {
                searchParams.PaymentTypeId = new System.Collections.Generic.List<int> { request.PaymentTypeId.Value };
            }
            if (request.IsPrint.HasValue)
            {
                searchParams.IsPrint = request.IsPrint;
            }
            if (request.IsCodFailedCollected.HasValue)
            {
                searchParams.IsCodFailedCollected = request.IsCodFailedCollected;
            }
            if (request.IsDocumentPod.HasValue)
            {
                searchParams.IsDocumentPod = request.IsDocumentPod;
            }
            if (!string.IsNullOrWhiteSpace(request.OrderCode))
            {
                searchParams.OptionValue = request.OrderCode;
            }
            if (request.FromDate.HasValue)
            {
                searchParams.FromTime = DateTimeHelper.ConvertToUnixTimestamp(request.FromDate.Value);
            }
            if (request.ToDate.HasValue)
            {
                searchParams.ToTime = DateTimeHelper.ConvertToUnixTimestamp(request.ToDate.Value);
            }

            var ghnOrdersResponse = await ghnApiClient.SearchOrdersAsync(apiConfig, searchParams);

            var (entityOrders, entityOrderItems) = await GHN_SyncOrderRequestHandler.ListOrderMappingAsync(
                ghnOrdersResponse,
                shopId,
                shopUniqueCode,
                apiConfig,
                ghnApiClient);

            return (entityOrders, entityOrderItems);
        }

        private static BaseResult<PaginationResponseDto<OrderDto>> MapPaginationResult(
            PaginationResponseDto<OrderDto> result,
            int skipCount,
            bool isAdmin,
            List<ShopDeliveryPricePlaneDto> orderDeiveryPricePlanDetails)
        {
            int index = 0;
            foreach (var item in result.Data)
            {
                item.No = skipCount + index + 1;

                // Hide some fields
                if (isAdmin == false)
                {
                    item.Height = item.RootHeight;
                    item.Length = item.RootLength;
                    item.Width = item.RootWidth;
                    item.Weight = item.RootWeight;
                    item.ConvertedWeight = item.RootConvertedWeight;
                    item.CalculateWeight = item.RootCalculateWeight;
                }

                if (item.DeliveryPricePlaneId.HasValue)
                {
                    item.OrderDeiveryPricePlanDetail = orderDeiveryPricePlanDetails.FirstOrDefault(i => i.Id == item.DeliveryPricePlaneId);
                }

                index++;
            }

            return BaseResult<PaginationResponseDto<OrderDto>>.Ok(result);
        }

        private static BaseResult<PaginationResponseDto<OrderDto>> EmptyResult(GetOrderPagedListRequest request)
        {
            return BaseResult<PaginationResponseDto<OrderDto>>.Ok(new PaginationResponseDto<OrderDto>(new List<OrderDto>(), 0, request.PageNumber, request.PageSize));
        }

        private static bool IsLocalQuery(OrderGroupStatus status) => status == OrderGroupStatus.ChoXacNhan || status == OrderGroupStatus.Nhap;
    }
}
