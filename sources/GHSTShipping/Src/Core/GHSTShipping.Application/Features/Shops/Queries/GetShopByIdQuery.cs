using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Queries
{
    public class GetShopByIdQuery : IRequest<List<BasicShopInfoDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetShopByIdQueryHandler(
        IShopRepository _shopRepository
        ) : IRequestHandler<GetShopByIdQuery, List<BasicShopInfoDto>>
    {
        public async Task<List<BasicShopInfoDto>> Handle(GetShopByIdQuery request, CancellationToken cancellationToken)
        {
            var shops = await _shopRepository
                .Where(s => s.Id == request.Id || s.ParentId == request.Id)
                .Select(s => new BasicShopInfoDto
                {
                    Id = s.Id,
                    ParentId = s.ParentId,
                    Name = s.Name,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    IsVerified = s.IsVerified,
                    WardId = s.WardId,
                    WardName = s.WardName,
                    DistrictId = s.DistrictId,
                    DistrictName = s.DistrictName,
                    ProvinceId = s.ProvinceId,
                    ProvinceName = s.ProvinceName
                })
                .ToListAsync(cancellationToken);

            return shops;
        }
    }
}
