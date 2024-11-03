using AutoMapper;
using AutoMapper.QueryableExtensions;
using Delivery.GHN;
using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.Extensions;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Parameters;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Queries
{
    public class GetOrderPagedListRequest : PaginationRequestParameter, IRequest<BaseResult<PaginationResponseDto<OrderDto>>>
    {
        public string DeliveryPartner { get; set; }

        public string OrderCode { get; set; }

        /// <summary>
        /// DRAFT | WAIT_CONFIRM | DELIVERING | RETURN | WAIT_CONFIRM_DELIVERY | COMPLETED | CANCEL | LOST
        /// </summary>
        public string Status { get; set; }

        public Guid? ShopId { get; set; }
    }

    public class GetOrderPagedListRequestHandler(
        IGhnApiClient ghnApiClient,
        IUnitOfWork unitOfWork,
        IAuthenticatedUserService authenticatedUser,
        IShopRepository shopRepository,
        MapperConfiguration mapperConfiguration
        ) : IRequestHandler<GetOrderPagedListRequest, BaseResult<PaginationResponseDto<OrderDto>>>
    {
        public async Task<BaseResult<PaginationResponseDto<OrderDto>>> Handle(GetOrderPagedListRequest request, CancellationToken cancellationToken)
        {
            var userId = authenticatedUser.UId;
            var isAdmin = authenticatedUser.Type == AccountTypeConstants.ADMIN;

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
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (request.Status == EnumOrderStatusConstants.DRAFT)
                {
                    query = query.Where(o => o.IsPublished == false || o.CurrentStatus == OrderStatus.WAITING_CONFIRM);
                }
                else
                {
                    query = query.Where(o => o.IsPublished == true && o.CurrentStatus == request.Status);
                }
            }

            query = query.OrderByDescending(i => i.Created);

            int skipCount = (request.PageNumber - 1) * request.PageSize;
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


        public async Task FetchOrdersFromGhnAsync()
        {
            //var response = await ghnApiClient.
        }
    }
}
