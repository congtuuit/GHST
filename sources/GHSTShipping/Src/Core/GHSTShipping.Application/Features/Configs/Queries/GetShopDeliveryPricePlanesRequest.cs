using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
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
    }

    public class ShopDeliveryPricePlaneDto
    {
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? ShopId { get; set; }
        public string Name { get; set; }
        public long MinWeight { get; set; }
        public long MaxWeight { get; set; }
        public long PublicPrice { get; set; }
        public long PrivatePrice { get; set; }
        public long StepPrice { get; set; }
        public long StepWeight { get; set; }
        public long LimitInsurance { get; set; }
        public decimal InsuranceFeeRate { get; set; }
        public decimal ReturnFeeRate { get; set; }
        public decimal ConvertWeightRate { get; set; }
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

            if (request.IsActivated.HasValue)
            {
                query = query.Where(i => i.IsActivated == request.IsActivated);
            }

            // Truy vấn kết hợp và mapping chỉ trong một lần
            var deliveryPricePlanes = await query
                .AsNoTracking() // Tối ưu hóa hiệu năng cho các truy vấn chỉ đọc
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


            if (request.ShopId.HasValue && deliveryPricePlanes.Count > 0)
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
