using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Commands
{
    public class CreateShopPriceCommand : CreateShopPriceRequest, IRequest<BaseResult>
    {
    }

    public class CreateShopPriceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateShopPriceCommand, BaseResult>
    {
        private static int MAX_LOOP = 50;

        public async Task<BaseResult> Handle(CreateShopPriceCommand request, CancellationToken cancellationToken)
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

        public async Task<BaseResult> HandleCreateShopPriceAsync(CreateShopPriceCommand request, CancellationToken cancellationToken)
        {
            var shopPricePlanes = new List<ShopPricePlan>();
            if (request.Mode == "mutilple")
            {
                var currentPrice = request.OfficialPrice;
                var currentConvertedWeight = new ShopPricePlan().CalcConvertedWeight(request.Length, request.Width, request.Height);
                var maxConvertedWeight = request.MaxConvertedWeight;
                int loop = 0;
                while (maxConvertedWeight > currentConvertedWeight)
                {
                    if (loop >= MAX_LOOP)
                    {
                        break;
                    }

                    var pricePlan = new ShopPricePlan(
                       request.ShopId,
                       request.Supplier,
                       request.PrivatePrice,
                       currentPrice,
                       request.Weight,
                       request.Length,
                       request.Width,
                       request.Height,
                       currentConvertedWeight
                    );

                    shopPricePlanes.Add(pricePlan);

                    // Next price plan
                    currentPrice += request.StepPrice;

                    // Next converted weight
                    currentConvertedWeight += request.StepWeight;

                    loop++;
                }
            }
            else
            {
                shopPricePlanes.Add(new ShopPricePlan(
                    request.ShopId,
                    request.Supplier,
                    request.PrivatePrice,
                    request.OfficialPrice,
                    request.Weight,
                    request.Length,
                    request.Width,
                    request.Height
                    ));
            }

            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await unitOfWork.ShopPricePlanes.AddRangeAsync(shopPricePlanes);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return BaseResult.Ok();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            return BaseResult.Failure();
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
                    Weight = i.Weight,
                    Length = i.Length,
                    Width = i.Width,
                    Height = i.Height,
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
                    pricePlan.Weight = request.Weight;
                    pricePlan.Length = request.Length;
                    pricePlan.Width = request.Width;
                    pricePlan.Height = request.Height;
                    pricePlan.CalcConvertedWeight();

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
