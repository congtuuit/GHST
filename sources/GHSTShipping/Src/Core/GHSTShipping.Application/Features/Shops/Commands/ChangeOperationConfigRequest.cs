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
    public class ChangeOperationConfigRequest : IRequest<BaseResult<ShopViewDetailDto>>
    {
        public Guid? ShopId { get; set; }

        public bool? AllowPublishOrder { get; set; }

        public bool? AllowUsePartnerShopAddress { get; set; }
    }

    public class ChangeOperationConfigRequestHandler(
       IUnitOfWork unitOfWork,
       IShopRepository shopRepository,
       IAccountServices accountServices,
       IMediator mediator
       ) : IRequestHandler<ChangeOperationConfigRequest, BaseResult<ShopViewDetailDto>>
    {
        public async Task<BaseResult<ShopViewDetailDto>> Handle(ChangeOperationConfigRequest request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository.Where(i => i.Id == request.ShopId)
                .Select(i => new Domain.Entities.Shop()
                {
                    Id = i.Id,
                    AllowPublishOrder = i.AllowPublishOrder,
                    AllowUsePartnerShopAddress = i.AllowUsePartnerShopAddress,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (shop != null)
            {
                shopRepository.Modify(shop);

                if (request.AllowPublishOrder.HasValue)
                {
                    shop.AllowPublishOrder = request.AllowPublishOrder.Value;
                }

                if (request.AllowUsePartnerShopAddress.HasValue)
                {
                    shop.AllowUsePartnerShopAddress = request.AllowUsePartnerShopAddress.Value;
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var result = await mediator.Send(new GetShopDetailRequest() { ShopId = request.ShopId });

            return result;
        }
    }
}
