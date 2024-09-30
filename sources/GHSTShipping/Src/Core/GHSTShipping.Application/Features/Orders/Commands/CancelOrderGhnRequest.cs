using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class CancelOrderGhnRequest : IRequest<BaseResult<CancelOrderResponse>>
    {
        public List<string> OrderCodes { get; set; }
    }

    public class CancelOrderGhnRequestHandler(IGhnApiClient ghnApiClient) : IRequestHandler<CancelOrderGhnRequest, BaseResult<CancelOrderResponse>>
    {
        private ApiConfig apiConfig;

        public async Task<BaseResult<CancelOrderResponse>> Handle(CancelOrderGhnRequest request, CancellationToken cancellationToken)
        {
            apiConfig = new ApiConfig("https://online-gateway.ghn.vn", "e62ef2ee-7ed4-11ef-aadd-aaeb25904740");
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
