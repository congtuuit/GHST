using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Commands
{
    public class CreateChildShopCommand : IRequest<BaseResult<Guid>>
    {
        public Guid? ShopId { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; } = null!;
        public string WardName { get; set; } = null!;
        public string WardId { get; set; }
        public string DistrictName { get; set; } = null!;
        public int? DistrictId { get; set; }
        public string ProvinceName { get; set; } = null!;
        public int? ProvinceId { get; set; }
    }

    public class CreateChildShopCommandHandler(
        IShopRepository shopRepository,
        IUnitOfWork unitOfWork,
        IAuthenticatedUserService authenticatedUserService,
        ILogger<CreateChildShopCommandHandler> _logger
        ) : IRequestHandler<CreateChildShopCommand, BaseResult<Guid>>
    {
        public async Task<BaseResult<Guid>> Handle(CreateChildShopCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Handle Create or Update logic based on the presence of request.Id
                var result = request.Id.HasValue
                    ? await UpdateShopAsync(request, cancellationToken)
                    : await CreateShopAsync(request, cancellationToken);

                if (result.Success)
                {
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    return result;
                }

                await transaction.RollbackAsync(cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, ex.Message);
                return new Error(ErrorCode.Exception, ex.Message);
            }
        }

        // Method to handle shop creation
        private async Task<BaseResult<Guid>> CreateShopAsync(CreateChildShopCommand request, CancellationToken cancellationToken)
        {
            var shop = new Domain.Entities.Shop
            {
                ParentId = request.ShopId,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                IsVerified = true,
                WardId = request.WardId,
                WardName = request.WardName,
                DistrictId = request.DistrictId,
                DistrictName = request.DistrictName,
                ProvinceId = request.ProvinceId,
                ProvinceName = request.ProvinceName,
            };

            await shopRepository.AddAsync(shop);

            return BaseResult<Guid>.Ok(shop.Id);
        }

        // Method to handle shop update
        private async Task<BaseResult<Guid>> UpdateShopAsync(CreateChildShopCommand request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository.GetByIdAsync(request.Id.Value);

            if (shop == null)
            {
                return new Error(ErrorCode.NotFound, "Shop không tồn tại.");
            }

            // Mapping updated fields
            shop.Name = request.Name;
            shop.PhoneNumber = request.PhoneNumber;
            shop.Address = request.Address;
            shop.WardId = request.WardId;
            shop.WardName = request.WardName;
            shop.DistrictId = request.DistrictId;
            shop.DistrictName = request.DistrictName;
            shop.ProvinceId = request.ProvinceId;
            shop.ProvinceName = request.ProvinceName;

            shopRepository.Update(shop); // Assuming Update method is available in the repository

            return BaseResult<Guid>.Ok(shop.Id);
        }
    }
}
