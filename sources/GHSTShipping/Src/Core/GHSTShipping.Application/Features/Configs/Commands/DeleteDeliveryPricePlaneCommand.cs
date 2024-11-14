using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Configs.Commands
{

    public class DeleteDeliveryPricePlaneCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteDeliveryPricePlaneCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteDeliveryPricePlaneHandler : IRequestHandler<DeleteDeliveryPricePlaneCommand, bool>
    {
        private readonly IDeliveryPricePlaneRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDeliveryPricePlaneHandler(IDeliveryPricePlaneRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDeliveryPricePlaneCommand request, CancellationToken cancellationToken)
        {
            var deliveryPricePlane = await _repository.GetByIdAsync(request.Id);

            if (deliveryPricePlane == null)
            {
                // Không tìm thấy đối tượng cần xóa
                return false;
            }

            // Thực hiện xóa
            _repository.HardDelete(deliveryPricePlane);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
