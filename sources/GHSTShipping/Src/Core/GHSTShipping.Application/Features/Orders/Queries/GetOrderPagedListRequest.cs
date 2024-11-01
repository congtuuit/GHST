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
        IUnitOfWork unitOfWork,
        IAuthenticatedUserService authenticatedUser,
        IShopRepository shopRepository
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

                query = query.Where(o => o.ShopId == shop.ShopId);
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
                    query = query.Where(o => o.IsPublished == true);
                }
            }

            int skipCount = (request.PageNumber - 1) * request.PageSize;
            PaginationResponseDto<OrderDto> pagingResult = await query
                .Select(i => new OrderDto
                {
                    Id = i.Id,
                    IsPublished = i.IsPublished,
                    PublishDate = i.PublishDate,
                    ShopId = i.ShopId,
                    ShopName = i.Shop.Name,
                    DeliveryPartner = i.DeliveryPartner,
                    DeliveryFee = i.DeliveryFee,
                    ClientOrderCode = i.ClientOrderCode,
                    FromName = i.FromName,
                    FromPhone = i.FromPhone,
                    FromAddress = i.FromAddress,
                    FromWardName = i.FromWardName,
                    FromDistrictName = i.FromDistrictName,
                    FromProvinceName = i.FromProvinceName,
                    ToName = i.ToName,
                    ToPhone = i.ToPhone,
                    ToAddress = i.ToAddress,
                    ToWardName = i.ToWardName,
                    ToDistrictName = i.ToDistrictName,
                    ToProvinceName = i.ToProvinceName,
                    Weight = i.Weight,
                    Length = i.Length,
                    Width = i.Width,
                    Height = i.Height,
                    CodAmount = i.CodAmount,
                    InsuranceValue = i.InsuranceValue,
                    ServiceTypeId = i.ServiceTypeId,
                    PaymentTypeId = i.PaymentTypeId,

                    Status = i.CurrentStatus
                })
                .ToPaginationAsync(request.PageNumber, request.PageSize, cancellationToken);

            int index = 0;
            foreach (var item in pagingResult.Data)
            {
                item.No = skipCount + index + 1;
                index++;
            }

            return BaseResult<PaginationResponseDto<OrderDto>>.Ok(pagingResult);
        }
    }
}
