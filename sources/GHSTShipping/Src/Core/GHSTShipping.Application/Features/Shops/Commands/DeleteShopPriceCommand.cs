using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Commands
{
    public class DeleteShopPriceCommand : IRequest<BaseResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteShopPriceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteShopPriceCommand, BaseResult>
    {
        public async Task<BaseResult> Handle(DeleteShopPriceCommand request, CancellationToken cancellationToken)
        {
            var pricePlan = await unitOfWork.ShopPricePlanes
                .Where(i => i.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (pricePlan == null)
            {
                return BaseResult.Failure();
            }

            try
            {
                unitOfWork.ShopPricePlanes.Delete(pricePlan);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex) { 
            
            }
           

            return BaseResult.Ok();
        }
    }
}
