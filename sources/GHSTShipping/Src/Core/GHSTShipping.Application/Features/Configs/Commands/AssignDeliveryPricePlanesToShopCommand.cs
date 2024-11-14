using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Configs.Commands
{
    public class AssignDeliveryPricePlanesToShopCommand : IRequest<bool>
    {
        public Guid ShopId { get; set; }
        public List<Guid> DeliveryPricePlaneIds { get; set; }

        public AssignDeliveryPricePlanesToShopCommand(Guid shopId, List<Guid> deliveryPricePlaneIds)
        {
            ShopId = shopId;
            DeliveryPricePlaneIds = deliveryPricePlaneIds;
        }
    }

    public class AssignDeliveryPricePlanesToShopHandler : IRequestHandler<AssignDeliveryPricePlanesToShopCommand, bool>
    {
        private readonly IDeliveryPricePlaneRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignDeliveryPricePlanesToShopHandler(IDeliveryPricePlaneRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AssignDeliveryPricePlanesToShopCommand request, CancellationToken cancellationToken)
        {
            var deliveryPricePlanes = await _repository
                .Where(i => request.DeliveryPricePlaneIds.Contains(i.Id))
                .Select(i => i.Id)
                .ToListAsync(cancellationToken);

            if (deliveryPricePlanes == null || !deliveryPricePlanes.Any())
            {
                throw new KeyNotFoundException("No DeliveryPricePlanes found with the provided Ids.");
            }

            var shopPricePlances = new List<DeliveryPricePlane>();
            foreach (var planeId in deliveryPricePlanes)
            {
                shopPricePlances.Add(new DeliveryPricePlane()
                {
                    ShopId = request.ShopId,
                    RelatedToDeliveryPricePlaneId = planeId,
                });
            }

            await _repository.AddRangeAsync(shopPricePlances);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
