using GHSTShipping.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Configs.Queries
{
    public class GetShopDeliveryPricePlanesRequest : IRequest<List<ShopDeliveryPricePlaneDto>>
    {
        public Guid ShopId { get; set; }
        public bool IsActivated { get; set; }
    }

    public class ShopDeliveryPricePlaneDto
    {
        public Guid? Id { get; set; }
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

    public class GetShopDeliveryPricePlanesRequestHandler : IRequestHandler<GetShopDeliveryPricePlanesRequest, List<ShopDeliveryPricePlaneDto>>
    {
        private readonly IDeliveryPricePlaneRepository _repository;

        public GetShopDeliveryPricePlanesRequestHandler(IDeliveryPricePlaneRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ShopDeliveryPricePlaneDto>> Handle(GetShopDeliveryPricePlanesRequest request, CancellationToken cancellationToken)
        {
            // Truy vấn kết hợp và mapping chỉ trong một lần
            var deliveryPricePlanes = await _repository
                .Where(i => (i.ShopId == request.ShopId && i.IsActivated == request.IsActivated)
                            || i.ShopId == null) // Bao gồm các giá trị gốc (root)
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
                })
                .ToListAsync(cancellationToken);

            return deliveryPricePlanes;
        }
    }
}
