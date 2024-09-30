using Delivery.GHN;
using Delivery.GHN.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
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
        public async Task<IActionResult> Provinces()
        {
            var response = await _ghnApiClient.GetProvinceAsync(apiConfig);

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> Dictricts()
        {
            var response = await _ghnApiClient.GetDistrictAsync(apiConfig, 269);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Wards()
        {
            var response = await _ghnApiClient.GetWardAsync(apiConfig, 2264);

            return Ok(response);
        }
    }
}
