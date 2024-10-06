using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.Extensions;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Parameters;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Queries
{
    public class GetOrderPagedListRequest : PaginationRequestParameter, IRequest<BaseResult<PaginationResponseDto<OrderDto>>>
    {
        public string DeliveryPartner { get; set; }

        public string OrderCode { get; set; }
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
            var shop = await shopRepository.Where(i => i.AccountId == userId)
            .Select(i => new
            {
                ShopId = i.Id,
                i.UniqueCode,
                i.AllowPublishOrder,
            })
           .FirstOrDefaultAsync(cancellationToken);

            var query = unitOfWork.Orders.All().Where(o => o.ShopId == shop.ShopId);
            if (!string.IsNullOrWhiteSpace(request.OrderCode))
            {
                query = query.Where(o => o.ClientOrderCode.Contains(request.OrderCode) || request.OrderCode.Contains(o.ClientOrderCode));
            }

            if (!string.IsNullOrWhiteSpace(request.DeliveryPartner))
            {
                query = query.Where(o => o.DeliveryPartner.Contains(request.DeliveryPartner));
            }

            int skipCount = (request.PageNumber - 1) * request.PageSize;
            PaginationResponseDto<OrderDto> pagingResult = await query
                .Select(i => new OrderDto
                {
                    Id = i.Id,
                    ClientOrderCode = i.ClientOrderCode,
                    IsPublished = i.IsPublished,
                    FromAddress = i.FromAddress,
                    ToAddress = i.ToAddress,
                    Weight = i.Weight,
                    PaymentTypeId = i.PaymentTypeId,
                    InsuranceValue = i.InsuranceValue,
                    CodAmount = i.CodAmount,
                    PublishDate = i.PublishDate,
                    DeliveryFee = i.DeliveryFee,
                    PrivateOrderCode = i.private_order_code,
                    //PrivateTotalFee = i.private_total_fee,
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
