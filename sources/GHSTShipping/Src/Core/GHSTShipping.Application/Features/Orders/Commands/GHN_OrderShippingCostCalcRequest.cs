﻿using GHSTShipping.Application.DTOs.Orders;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class GHN_OrderShippingCostCalcRequest : IRequest<OrderShippingCostDto>
    {
        public Guid DeliveryPricePlaneId { get; set; }
        public long Weight { get; set; } // Cân nặng của đơn hàng (gram)
        public long Length { get; set; } // Chiều dài của đơn hàng (cm)
        public long Width { get; set; } // Chiều rộng của đơn hàng (cm)
        public long Height { get; set; } // Chiều cao của đơn hàng (cm)

        /// <summary>
        /// Giá trị đơn hàng
        /// </summary>
        public long InsuranceValue { get; set; }
    }

    public class GHN_OrderShippingCostCalcRequestHandler : IRequestHandler<GHN_OrderShippingCostCalcRequest, OrderShippingCostDto>
    {
        private readonly IDeliveryPricePlaneRepository _repository;

        public GHN_OrderShippingCostCalcRequestHandler(IDeliveryPricePlaneRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderShippingCostDto> Handle(GHN_OrderShippingCostCalcRequest request, CancellationToken cancellationToken)
        {
            // Lấy thông tin bảng giá theo ShopDeliveryPricePlaneId
            var validPricePlane = await _repository
                .Where(x => x.Id == request.DeliveryPricePlaneId)
                .Select(x => new
                {
                    x.Id,
                    x.RelatedToDeliveryPricePlaneId,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (validPricePlane == null)
            {
                throw new Exception("Không tìm thấy bảng giá cho ShopDeliveryPricePlaneId đã cung cấp.");
            }

            var pricePlane = await _repository.Where(i => i.Id == validPricePlane.RelatedToDeliveryPricePlaneId)
                    .Select(i => new DeliveryPricePlane
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
                    .FirstOrDefaultAsync();

            if (pricePlane == null)
            {
                throw new Exception("Không tìm thấy bảng giá cho ShopDeliveryPricePlaneId đã cung cấp.");
            }

            var convertWeightRate = pricePlane.ConvertWeightRate <= 0 ? 1 : pricePlane.ConvertWeightRate;

            // Tính toán khối lượng chuyển đổi
            var convertedWeight = ((request.Length * request.Width * request.Height) / convertWeightRate) * 1000;

            // Lấy khối lượng cuối cùng là giá trị lớn nhất giữa cân nặng thực tế và cân nặng chuyển đổi
            var finalWeight = Math.Max(convertedWeight, request.Weight);

            // Tính giá giao hàng dựa trên khối lượng
            long shippingCost;

            if (finalWeight <= pricePlane.MaxWeight)
            {
                // Nếu khối lượng nằm trong khoảng MinWeight và MaxWeight
                shippingCost = pricePlane.PublicPrice;
            }
            else
            {
                // Nếu khối lượng lớn hơn MaxWeight, tính giá theo StepPrice và StepWeight
                var extraWeight = finalWeight - pricePlane.MaxWeight;
                var extraSteps = (long)Math.Ceiling((double)extraWeight / pricePlane.StepWeight);
                shippingCost = pricePlane.PublicPrice + (extraSteps * pricePlane.StepPrice);
            }

            decimal insuranceFee = 0;
            if (request.InsuranceValue > pricePlane.LimitInsurance)
            {
                insuranceFee = (pricePlane.InsuranceFeeRate / 100) * request.InsuranceValue;
            }

            // Trả về kết quả tính toán
            return new OrderShippingCostDto
            {
                ShopDeliveryPricePlaneId = request.DeliveryPricePlaneId,
                OrderWeight = finalWeight,
                ShippingCost = shippingCost,
                InsuranceFee = (long)insuranceFee,
                CalcOrderWeight = convertedWeight
            };
        }
    }
}
