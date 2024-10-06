using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class DeliveryController : BaseApiController
    {
        private readonly IGhnApiClient _ghnApiClient;
        private readonly ApiConfig apiConfig;

        public DeliveryController(IGhnApiClient ghnApiClient)
        {
            _ghnApiClient = ghnApiClient;
            apiConfig = new ApiConfig("https://online-gateway.ghn.vn", "e62ef2ee-7ed4-11ef-aadd-aaeb25904740");
        }

        [HttpGet]
        public async Task<BaseResult<IEnumerable<PickShiftResponse>>> PickShifts()
        {
            var response = await _ghnApiClient.GetPickShiftAsync(apiConfig);
            return BaseResult<IEnumerable<PickShiftResponse>>.Ok(response);
        }
    }
}
