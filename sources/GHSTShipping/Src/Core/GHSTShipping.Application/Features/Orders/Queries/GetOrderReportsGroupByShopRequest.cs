using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Queries
{
    public class GetOrderReportsGroupByShopRequest : IRequest<BaseResult<IEnumerable<ShopViewReportDto>>>
    {
    }

    public class ShopViewReportDto
    {
        public Guid Id { get; set; }
        public string UniqueCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int TotalDraftOrder { get; set; }
        public int TotalPublishedOrder { get; set; }
    }

    public class GetOrderReportsGroupByShopRequestHandler(
        IAuthenticatedUserService _authenticatedUserService,
        IShopRepository _shopRepository

        ) : IRequestHandler<GetOrderReportsGroupByShopRequest, BaseResult<IEnumerable<ShopViewReportDto>>>
    {

        public async Task<BaseResult<IEnumerable<ShopViewReportDto>>> Handle(GetOrderReportsGroupByShopRequest request, CancellationToken cancellationToken)
        {
            var shops = await _shopRepository.Where(i => i.IsVerified).Select(i => new ShopViewReportDto
            {
                Id = i.Id,
                UniqueCode = i.UniqueCode,
                Name = i.Name,
                TotalDraftOrder = i.Orders.Count(o => o.IsPublished == false),
                TotalPublishedOrder = i.Orders.Count(o => o.IsPublished == true)
            })
            .ToListAsync(cancellationToken);

            return BaseResult<IEnumerable<ShopViewReportDto>>.Ok(shops);
        }
    }
}
