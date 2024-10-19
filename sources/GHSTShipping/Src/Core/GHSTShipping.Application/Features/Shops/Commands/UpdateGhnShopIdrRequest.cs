using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Features.Shops.Queries;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Commands
{
    public class UpdateGhnShopIdrRequest : IRequest<BaseResult<ShopViewDetailDto>>
    {
        public Guid? ShopId { get; set; }

        public int? GhnShopId { get; set; }
    }

    public class UpdateGhnShopIdrRequestHandler(
       IUnitOfWork unitOfWork,
       IShopRepository shopRepository,
       IAccountServices accountServices,
       IMediator mediator
       ) : IRequestHandler<UpdateGhnShopIdrRequest, BaseResult<ShopViewDetailDto>>
    {
        public async Task<BaseResult<ShopViewDetailDto>> Handle(UpdateGhnShopIdrRequest request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository.Where(i => i.Id == request.ShopId)
                .Select(i => new Domain.Entities.Shop()
                {
                    Id = i.Id,
                    GhnShopId = i.GhnShopId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (shop != null)
            {
                shopRepository.Modify(shop);
                shop.GhnShopId = request.GhnShopId;
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var result = await mediator.Send(new GetShopDetailRequest() { ShopId = request.ShopId });

            return result;
        }
    }
}
