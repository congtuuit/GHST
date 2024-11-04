using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Commands
{
    public class DeleteShopPriceCommand : IRequest<BaseResult>
    {
        public Guid Id { get; set; }

        public IEnumerable<Guid> Ids { get; set; } = null;
    }

    public class DeleteShopPriceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteShopPriceCommand, BaseResult>
    {
        public async Task<BaseResult> Handle(DeleteShopPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Ids != null && request.Ids.Any())
                {
                    var pricePlanes = await unitOfWork.ShopPricePlanes
                        .Where(i => request.Ids.Contains(i.Id))
                        .ToListAsync(cancellationToken);

                    if (pricePlanes.Count != 0)
                    {
                        unitOfWork.ShopPricePlanes.HardDeleteRange(pricePlanes);
                    }
                }
                else
                {
                    var pricePlan = await unitOfWork.ShopPricePlanes
                        .Where(i => i.Id == request.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (pricePlan != null)
                    {
                        unitOfWork.ShopPricePlanes.HardDelete(pricePlan);
                    }
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

            }

            return BaseResult.Ok();
        }
    }
}
