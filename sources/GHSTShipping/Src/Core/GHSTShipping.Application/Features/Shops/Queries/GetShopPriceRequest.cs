using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.Extensions;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Parameters;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Queries
{
    public class GetShopPriceRequest : PaginationRequestParameter, IRequest<BaseResult<PaginationResponseDto<ShopPricePlanDto>>>
    {
        public Guid ShopId { get; set; }

        public string Supplier { get; set; }
    }

    public class GetShopPriceRequestHandler(
        IUnitOfWork unitOfWork
        ) : IRequestHandler<GetShopPriceRequest, BaseResult<PaginationResponseDto<ShopPricePlanDto>>>
    {
        public async Task<BaseResult<PaginationResponseDto<ShopPricePlanDto>>> Handle(GetShopPriceRequest request, CancellationToken cancellationToken)
        {
            int skipCount = (request.PageNumber - 1) * request.PageSize;

            var query = unitOfWork.ShopPricePlanes.Where(i => i.ShopId == request.ShopId);
            if (!string.IsNullOrWhiteSpace(request.Supplier))
            {
                var suppliers = request.Supplier.Split(",");
                query = query.Where(i => suppliers.Contains(i.Supplier));
            }

            PaginationResponseDto<ShopPricePlanDto> pagingResult = await query
                .Select(i => new ShopPricePlanDto
                {
                    Id = i.Id,
                    ShopId = i.ShopId,
                    ShopName = i.Shop.Name,
                    ShopUniqueCode = i.Shop.UniqueCode,
                    Weight = i.Weight,
                    Length = i.Length,
                    Width = i.Width,
                    Height = i.Height,
                    ConvertedWeight = i.ConvertedWeight,
                    OfficialPrice = i.OfficialPrice,
                    PrivatePrice = i.PrivatePrice,
                    Supplier = i.Supplier,
                })
                .OrderBy(i => i.ConvertedWeight)
                .ToPaginationAsync(request.PageNumber, request.PageSize, cancellationToken);

            int index = 0;
            foreach (var item in pagingResult.Data)
            {
                item.No = skipCount + index + 1;
                index++;
            }

            return BaseResult<PaginationResponseDto<ShopPricePlanDto>>.Ok(pagingResult);
        }
    }
}
