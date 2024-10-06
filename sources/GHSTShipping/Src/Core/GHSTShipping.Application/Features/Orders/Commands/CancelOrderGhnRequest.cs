using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class CancelOrderGhnRequest : IRequest<BaseResult<CancelOrderResponse>>
    {
        public List<string> OrderCodes { get; set; }
    }

    public class CancelOrderGhnRequestHandler(
        IGhnApiClient ghnApiClient,
        IPartnerConfigService partnerConfigService,
        IAuthenticatedUserService authenticatedUserService,
        IShopRepository shopRepository
        ) : IRequestHandler<CancelOrderGhnRequest, BaseResult<CancelOrderResponse>>
    {
        private ApiConfig apiConfig;

        public async Task<BaseResult<CancelOrderResponse>> Handle(CancelOrderGhnRequest request, CancellationToken cancellationToken)
        {
            var userId = authenticatedUserService.UId;
            var shop = await shopRepository.Where(i => i.AccountId == userId).Select(i => new
            {
                ShopId = i.Id,
                i.UniqueCode,
                i.AllowPublishOrder,
            })
            .FirstOrDefaultAsync(cancellationToken);

            // Get API config to send request to GHN
            var partnerConfig = await partnerConfigService.GetPartnerConfigAsync(Domain.Enums.EnumDeliveryPartner.GHN);
            var apiConfig = new ApiConfig(partnerConfig.ProdEnv, partnerConfig.ApiKey);

            var apiResult = await ghnApiClient.CancelOrderAsync(apiConfig, request.OrderCodes);
            if (apiResult.Code == 200)
            {
                return BaseResult<CancelOrderResponse>.Ok(apiResult.Data);
            }
            else
            {
                return BaseResult<CancelOrderResponse>.Failure(new Error(ErrorCode.ErrorInIdentity, apiResult.Message));
            }
        }
    }
}
