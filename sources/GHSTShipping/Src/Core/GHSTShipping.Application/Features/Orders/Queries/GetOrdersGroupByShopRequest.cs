using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.Extensions;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Parameters;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Queries
{
    public class GetOrdersGroupByShopRequest : PaginationRequestParameter, IRequest<BaseResult<PaginationResponseDto<ShopViewReportDto>>>
    {
    }

    public class ShopViewReportDto
    {
        public int No { get; set; }
        public Guid Id { get; set; }
        public string UniqueCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int TotalDraftOrder { get; set; }
        public int TotalPublishedOrder { get; set; }
    }

    public class GetOrderReportsGroupByShopRequestHandler(
        IAuthenticatedUserService _authenticatedUserService,
        IShopRepository _shopRepository,
        IShopPartnerConfigRepository _shopPartnerConfigRepository
        ) : IRequestHandler<GetOrdersGroupByShopRequest, BaseResult<PaginationResponseDto<ShopViewReportDto>>>
    {

        public async Task<BaseResult<PaginationResponseDto<ShopViewReportDto>>> Handle(GetOrdersGroupByShopRequest request, CancellationToken cancellationToken)
        {
            var paginationResponse = await _shopRepository
                .Where(i => i.IsVerified)
                .Select(i => new ShopViewReportDto
                {
                    Id = i.Id,
                    UniqueCode = i.UniqueCode,
                    Name = i.Name,
                    TotalDraftOrder = i.Orders.Count(o => o.IsPublished == false && o.CurrentStatus != OrderStatus.CANCEL),
                    TotalPublishedOrder = i.Orders.Count(o => o.IsPublished == true && o.CurrentStatus != OrderStatus.CANCEL)
                })
                .ToPaginationAsync(request.PageNumber, request.PageSize);

            var shops = paginationResponse.Data;

            // Get shop detail connected with delivery partner
            var shopDetails = await _shopPartnerConfigRepository.Where(i => shops.Select(s => s.Id).Contains(i.ShopId))
                .Select(i => new
                {
                    i.Id,
                    i.ShopId,
                    i.Address,
                    i.DistrictName,
                    i.WardName,
                    i.ProvinceName
                })
                .ToListAsync();

            int index = 0;
            int skipCount = (request.PageNumber - 1) * request.PageSize;
            foreach (var shop in shops)
            {
                shop.No = skipCount + index + 1;

                var shopDetail = shopDetails.FirstOrDefault(i => i.ShopId == shop.Id);
                if (shopDetail != null)
                {
                    shop.Address = AddressFormatted(shopDetail.Address, shopDetail.WardName, shopDetail.DistrictName, shopDetail.ProvinceName);
                }

                index++;
            }

            return BaseResult<PaginationResponseDto<ShopViewReportDto>>.Ok(paginationResponse);
        }

        public string AddressFormatted(string address, string wardName, string districtName, string provinceName)
        {
            var addressArr = new List<string>();
            if (!string.IsNullOrWhiteSpace(address))
            {
                addressArr.Add(address);
            }

            if (!string.IsNullOrWhiteSpace(wardName))
            {
                addressArr.Add(wardName);
            }

            if (!string.IsNullOrWhiteSpace(districtName))
            {
                addressArr.Add(districtName);
            }

            if (!string.IsNullOrWhiteSpace(provinceName))
            {
                addressArr.Add(provinceName);
            }

            return string.Join(", ", addressArr);
        }
    }
}
