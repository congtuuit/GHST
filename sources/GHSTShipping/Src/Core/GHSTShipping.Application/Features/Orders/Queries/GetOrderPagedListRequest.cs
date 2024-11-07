using AutoMapper;
using AutoMapper.QueryableExtensions;
using Delivery.GHN;
using Delivery.GHN.Constants;
using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.Extensions;
using GHSTShipping.Application.Features.Orders.Commands;
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
        public OrderGroupStatus Status { get; set; }

        /// <summary>
        /// The partner order code like codeA,codeB,...
        /// </summary>
        public string OrderCode { get; set; }

        #endregion

        public string DeliveryPartner { get; set; }

        public Guid? ShopId { get; set; }
    }

    public class GetOrderPagedListRequestHandler(
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
            var userId = authenticatedUser.UId;
            var isAdmin = authenticatedUser.Type == AccountTypeConstants.ADMIN;
            var shopId = request.ShopId.Value;
            int skipCount = (request.PageNumber - 1) * request.PageSize;

            var query = unitOfWork.Orders.All();
            if (!isAdmin)
            {
                var shop = await shopRepository.Where(i => i.AccountId == userId)
                  .Select(i => new
                  {
                      ShopId = i.Id,
                      i.UniqueCode,
                      i.AllowPublishOrder,
                  })
                 .FirstOrDefaultAsync(cancellationToken);

                if (shop == null)
                {
                    return BaseResult<PaginationResponseDto<OrderDto>>.Ok(new PaginationResponseDto<OrderDto>(new System.Collections.Generic.List<OrderDto>(), 0, request.PageNumber, request.PageSize));
                }
            }

            // Filter by shopId
            if (request.ShopId.HasValue)
            {
                query = query.Where(i => i.ShopId == request.ShopId);
            }

            // Filter by code
            if (!string.IsNullOrWhiteSpace(request.OrderCode))
            {
                query = query.Where(o => o.ClientOrderCode.Contains(request.OrderCode) || request.OrderCode.Contains(o.ClientOrderCode));
            }

            // Filter by delivery partner
            if (!string.IsNullOrWhiteSpace(request.DeliveryPartner))
            {
                query = query.Where(o => o.DeliveryPartner.Contains(request.DeliveryPartner));
            }

            // Filter by status
            if (request.Status == OrderGroupStatus.Nhap)
            {
                query = query.Where(o => o.IsPublished == false || o.CurrentStatus == OrderStatus.WAITING_CONFIRM);
                query = query.OrderByDescending(i => i.Created);

                PaginationResponseDto<OrderDto> pagingResult = await query
                    .ProjectTo<OrderDto>(mapperConfiguration)
                    .ToPaginationAsync(request.PageNumber, request.PageSize, cancellationToken);

                int index = 0;
                foreach (var item in pagingResult.Data)
                {
                    item.No = skipCount + index + 1;
                    index++;
                }

                return BaseResult<PaginationResponseDto<OrderDto>>.Ok(pagingResult);
            }
            else
            {
                if (request.DeliveryPartner == EnumDeliveryPartner.GHN.GetCode())
                {
                    var apiConfig = await partnerConfigService.GetApiConfigAsync(Domain.Enums.EnumDeliveryPartner.GHN, shopId);
                    var searchStatus = request.Status.GetDetails();
                    var searchParams = new Delivery.GHN.Models.ShippingOrderSearchRequest
                    {
                        Status = searchStatus,
                        ShopId = int.Parse(apiConfig.ShopId),
                        Limit = request.PageSize,
                        Offset = request.PageNumber > 0 ? request.PageNumber - 1 : request.PageNumber,
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

                    var ghnOrdersResponse = await ghnApiClient.SearchOrdersAsync(apiConfig, searchParams);
                    var (entityOrders, entityOrderItems) = await GHN_SyncOrderRequestHandler.ListOrderMappingAsync(ghnOrdersResponse, shopId, apiConfig, ghnApiClient);

                    //// TODO open to optimize performance
                    //Task.Run(() => JobSyncOrders(entityOrders, entityOrderItems));

                    await GHN_SyncOrderRequestHandler.BatchSaveAsync(unitOfWork, entityOrders, entityOrderItems);

                    var _orders = mapper.Map<List<OrderDto>>(entityOrders);
                    var result = new PaginationResponseDto<OrderDto>(_orders, ghnOrdersResponse.Total, request.PageNumber, request.PageSize);

                    int index = 0;
                    foreach (var item in result.Data)
                    {
                        item.No = skipCount + index + 1;
                        if (string.IsNullOrWhiteSpace(item.ClientOrderCode))
                        {
                            item.ClientOrderCode = item.PrivateOrderCode;
                        }
                        index++;
                    }

                    return BaseResult<PaginationResponseDto<OrderDto>>.Ok(result);
                }
            }

            return BaseResult<PaginationResponseDto<OrderDto>>.Ok(null);
        }

        private async Task JobSyncOrders(List<Order> orders, List<OrderItem> orderItems)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                await GHN_SyncOrderRequestHandler.BatchSaveAsync(unitOfWork, orders, orderItems);
            }
        }
    }
}
