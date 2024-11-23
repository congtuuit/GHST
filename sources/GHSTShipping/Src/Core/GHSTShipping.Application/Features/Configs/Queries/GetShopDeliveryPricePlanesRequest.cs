using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Configs.Queries
{
    public class GetShopDeliveryPricePlanesRequest : IRequest<BaseResult<List<ShopDeliveryPricePlaneDto>>>
    {
        public Guid? ShopId { get; set; }
        public bool? IsActivated { get; set; }

        public IEnumerable<Guid> Ids { get; set; }
    }

    public class GetShopDeliveryPricePlanesRequestHandler : IRequestHandler<GetShopDeliveryPricePlanesRequest, BaseResult<List<ShopDeliveryPricePlaneDto>>>
    {
        private readonly IDeliveryPricePlaneRepository _repository;

        public GetShopDeliveryPricePlanesRequestHandler(IDeliveryPricePlaneRepository repository)
        {
            _repository = repository;
        }

        public async Task<BaseResult<List<ShopDeliveryPricePlaneDto>>> Handle(GetShopDeliveryPricePlanesRequest request, CancellationToken cancellationToken)
        {
            var query = _repository.Where(i => i.ShopId == null);

            if (request.ShopId.HasValue)
            {
                query = _repository.Where(i => i.ShopId == request.ShopId);
            }

            if (request.Ids != null && request.Ids.Any())
            {
                query = _repository.Where(i => request.Ids.Contains(i.Id));
            }

            if (request.IsActivated.HasValue)
            {
                query = query.Where(i => i.IsActivated == request.IsActivated);
            }

            var deliveryPricePlanes = await query
                .AsNoTracking()
                .Select(i => new ShopDeliveryPricePlaneDto
                {
                    Id = i.Id,
                    ShopId = i.ShopId,
                    Name = i.Name,
                    MinWeight = i.MinWeight,
                    MaxWeight = i.MaxWeight,
                    PublicPrice = i.PublicPrice,
                    PrivatePrice = i.PrivatePrice,
                    StepPrice = i.StepPrice,
                    StepWeight = i.StepWeight,
                    LimitInsurance = i.LimitInsurance,
                    InsuranceFeeRate = i.InsuranceFeeRate,
                    ReturnFeeRate = i.ReturnFeeRate,
                    ConvertWeightRate = i.ConvertWeightRate,

                    ParentId = i.RelatedToDeliveryPricePlaneId
                })
                .ToListAsync(cancellationToken);

            bool fetchDetail = (request.ShopId.HasValue && deliveryPricePlanes.Count > 0) || (request.Ids != null && request.Ids.Any());
            if (fetchDetail)
            {
                var parentPricePlaneIds = deliveryPricePlanes.Select(i => i.ParentId);
                var parentPricePlanes = await _repository.Where(i => parentPricePlaneIds.Contains(i.Id))
                    .Select(i => new ShopDeliveryPricePlaneDto
                    {
                        Id = i.Id,
                        ShopId = i.ShopId,
                        Name = i.Name,
                        MinWeight = i.MinWeight,
                        MaxWeight = i.MaxWeight,
                        PublicPrice = i.PublicPrice,
                        PrivatePrice = i.PrivatePrice,
                        StepPrice = i.StepPrice,
                        StepWeight = i.StepWeight,
                        LimitInsurance = i.LimitInsurance,
                        InsuranceFeeRate = i.InsuranceFeeRate,
                        ReturnFeeRate = i.ReturnFeeRate,
                        ConvertWeightRate = i.ConvertWeightRate,
                        ParentId = i.RelatedToDeliveryPricePlaneId
                    })
                    .ToListAsync();

                foreach (var item in deliveryPricePlanes)
                {
                    var parent = parentPricePlanes.FirstOrDefault(i => i.Id == item.ParentId);
                    if (parent != null)
                    {
                        item.Name = parent.Name;
                        item.MinWeight = parent.MinWeight;
                        item.MaxWeight = parent.MaxWeight;
                        item.PublicPrice = parent.PublicPrice;
                        item.PrivatePrice = parent.PrivatePrice;
                        item.StepPrice = parent.StepPrice;
                        item.StepWeight = parent.StepWeight;
                        item.LimitInsurance = parent.LimitInsurance;
                        item.InsuranceFeeRate = parent.InsuranceFeeRate;
                        item.ReturnFeeRate = parent.ReturnFeeRate;
                        item.ConvertWeightRate = parent.ConvertWeightRate;
                    }
                }
            }

            return BaseResult<List<ShopDeliveryPricePlaneDto>>.Ok(deliveryPricePlanes);
        }
    }
}
