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
    public class AllowPublishOrderRequest : IRequest<BaseResult<ShopViewDetailDto>>
    {
        public Guid? ShopId { get; set; }
    }

    public class AllowPublishOrderRequestHandler(
       IUnitOfWork unitOfWork,
       IShopRepository shopRepository,
       IAccountServices accountServices,
       IMediator mediator
       ) : IRequestHandler<AllowPublishOrderRequest, BaseResult<ShopViewDetailDto>>
    {
        public async Task<BaseResult<ShopViewDetailDto>> Handle(AllowPublishOrderRequest request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository.Where(i => i.Id == request.ShopId)
                .Select(i => new Domain.Entities.Shop()
                {
                    Id = i.Id,
                    AllowPublishOrder = i.AllowPublishOrder,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (shop != null)
            {
                shopRepository.Modify(shop);
                shop.AllowPublishOrder = !shop.AllowPublishOrder;
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var result = await mediator.Send(new GetShopDetailRequest() { ShopId = request.ShopId });

            return result;
        }
    }
}
