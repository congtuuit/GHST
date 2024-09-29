using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Commands
{
    public class CreateShopPriceCommand : CreateShopPriceRequest, IRequest<BaseResult<Guid>>
    {
    }

    public class CreateShopPriceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateShopPriceCommand, BaseResult<Guid>>
    {
        public async Task<BaseResult<Guid>> Handle(CreateShopPriceCommand request, CancellationToken cancellationToken)
        {
            var suppliers = EnumSupplierExtension.GetSuppliers();
            if (suppliers.Contains(request.Supplier) == false)
            {
                return new Error(ErrorCode.FieldDataInvalid);
            }

            if (request.Id.HasValue)
            {
                return await HandleUpdateShopPriceAsunc(request, cancellationToken);
            }
            else
            {
                return await HandleCreateShopPriceAsync(request, cancellationToken);
            }
        }

        public async Task<BaseResult<Guid>> HandleCreateShopPriceAsync(CreateShopPriceCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var newPricePlan = new ShopPricePlan(
                    request.ShopId,
                    request.Supplier,
                    request.PrivatePrice,
                    request.OfficialPrice,
                    request.Capacity
                    );

                await unitOfWork.ShopPricePlanes.AddAsync(newPricePlan);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return BaseResult<Guid>.Ok(newPricePlan.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            return BaseResult<Guid>.Failure();
        }

        public async Task<BaseResult<Guid>> HandleUpdateShopPriceAsunc(CreateShopPriceCommand request, CancellationToken cancellationToken)
        {
            var pricePlan = await unitOfWork.ShopPricePlanes
                .Where(i => i.Id == request.Id)
                .Select(i => new ShopPricePlan
                {
                    Id = i.Id,
                    Supplier = i.Supplier,
                    PrivatePrice = i.PrivatePrice,
                    OfficialPrice = i.OfficialPrice,
                    Capacity = i.Capacity,
                })
                .FirstOrDefaultAsync();

            if (pricePlan == null)
            {
                return new Error(ErrorCode.NotFound);
            }
            else
            {
                using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    unitOfWork.ShopPricePlanes.Modify(pricePlan);
                    pricePlan.Supplier = request.Supplier;
                    pricePlan.PrivatePrice = request.PrivatePrice;
                    pricePlan.OfficialPrice = request.OfficialPrice;
                    pricePlan.Capacity = request.Capacity;

                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    return BaseResult<Guid>.Ok(pricePlan.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                }

                return BaseResult<Guid>.Failure();
            }
        }
    }
}
