using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Configs.Commands
{
    public class UpsertDeliveryPricePlaneCommand : IRequest<Guid>
    {
        public Guid? Id { get; set; } // Nếu Id có giá trị, sẽ thực hiện update, nếu null thì tạo mới
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
        public bool IsActivated { get; set; }
    }

    public class UpsertDeliveryPricePlaneHandler : IRequestHandler<UpsertDeliveryPricePlaneCommand, Guid>
    {
        private readonly IDeliveryPricePlaneRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpsertDeliveryPricePlaneHandler(IDeliveryPricePlaneRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpsertDeliveryPricePlaneCommand request, CancellationToken cancellationToken)
        {
            DeliveryPricePlane deliveryPricePlane;

            if (request.Id.HasValue)
            {
                deliveryPricePlane = await _repository.GetByIdAsync(request.Id.Value);
                if (deliveryPricePlane == null)
                {
                    throw new KeyNotFoundException($"Delivery Price Plane with Id {request.Id} not found.");
                }
            }
            else
            {
                deliveryPricePlane = new DeliveryPricePlane();
            }

            deliveryPricePlane.ShopId = request.ShopId;
            deliveryPricePlane.Name = request.Name;
            deliveryPricePlane.MinWeight = request.MinWeight;
            deliveryPricePlane.MaxWeight = request.MaxWeight;
            deliveryPricePlane.PublicPrice = request.PublicPrice;
            deliveryPricePlane.PrivatePrice = request.PrivatePrice;
            deliveryPricePlane.StepPrice = request.StepPrice;
            deliveryPricePlane.StepWeight = request.StepWeight;
            deliveryPricePlane.LimitInsurance = request.LimitInsurance;
            deliveryPricePlane.InsuranceFeeRate = request.InsuranceFeeRate;
            deliveryPricePlane.ReturnFeeRate = request.ReturnFeeRate;
            deliveryPricePlane.ConvertWeightRate = request.ConvertWeightRate;
            deliveryPricePlane.IsActivated = request.IsActivated;

            if (request.Id.HasValue)
            {
                _repository.Update(deliveryPricePlane);
            }
            else
            {
                await _repository.AddAsync(deliveryPricePlane);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return deliveryPricePlane.Id;
        }
    }


}
