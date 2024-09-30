using AutoMapper;
using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class CreateGhnOrderRequest : CreateDeliveryOrderRequest, IRequest<BaseResult<CreateDeliveryOrderResponse>>
    {
        public int ShopId { get; set; }
    }

    public class CreateGhnOrderRequestHandler : IRequestHandler<CreateGhnOrderRequest, BaseResult<CreateDeliveryOrderResponse>>
    {
        private readonly IGhnApiClient _ghnApiClient;
        private readonly ApiConfig apiConfig;
        private readonly IMapper _mapper;

        public CreateGhnOrderRequestHandler(IGhnApiClient ghnApiClient, IMapper mapper)
        {
            _mapper = mapper;
            _ghnApiClient = ghnApiClient;
            apiConfig = new ApiConfig("https://online-gateway.ghn.vn", "e62ef2ee-7ed4-11ef-aadd-aaeb25904740");
        }

        public async Task<BaseResult<CreateDeliveryOrderResponse>> Handle(CreateGhnOrderRequest request, CancellationToken cancellationToken)
        {
            int weight = request.weight;
            // TODO get new fee from system config

            var deliveryOrderRequest = _mapper.Map<CreateDeliveryOrderRequest>(request);
            var apiResult = await _ghnApiClient.CreateDraftDeliveryOrderAsync(apiConfig, request.ShopId, deliveryOrderRequest);
            if (apiResult.Code == 200)
            {
                return BaseResult<CreateDeliveryOrderResponse>.Ok(apiResult.Data);
            }

            return BaseResult<CreateDeliveryOrderResponse>.Failure(new Error(ErrorCode.ErrorInIdentity, apiResult.Message));
        }
    }
}
