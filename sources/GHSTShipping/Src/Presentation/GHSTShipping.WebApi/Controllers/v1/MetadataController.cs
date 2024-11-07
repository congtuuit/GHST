using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.Helpers;
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
    public class MetadataController : BaseApiController
    {
        private readonly IGhnApiClient _ghnApiClient;
        private readonly ApiConfig apiConfig;

        public MetadataController(IGhnApiClient ghnApiClient)
        {
            _ghnApiClient = ghnApiClient;
            apiConfig = new ApiConfig("https://online-gateway.ghn.vn", "e62ef2ee-7ed4-11ef-aadd-aaeb25904740");
        }

        [HttpGet]
        public async Task<BaseResult<IEnumerable<ProvinceResponse>>> Provinces()
        {
            var response = await _ghnApiClient.GetProvinceAsync(apiConfig);
            return BaseResult<IEnumerable<ProvinceResponse>>.Ok(response);
        }

        [HttpGet]
        public async Task<BaseResult<IEnumerable<DistrictResponse>>> Dictricts()
        {
            var provinces = await _ghnApiClient.GetProvinceAsync(apiConfig);
            var dictricts = await _ghnApiClient.GetDistrictAsync(apiConfig);
            var response = dictricts.ToList();
            foreach (var dictrict in response)
            {
                var province = provinces.FirstOrDefault(i => i.ProvinceId == dictrict.ProvinceID);
                if (province == null) continue;

                dictrict.ProvinceName = province.ProvinceName;
            }

            return BaseResult<IEnumerable<DistrictResponse>>.Ok(response);
        }

        [HttpGet]
        public async Task<BaseResult<IEnumerable<WardResponse>>> Wards(int districtId)
        {
            var response = await _ghnApiClient.GetWardAsync(apiConfig, districtId);
            return BaseResult<IEnumerable<WardResponse>>.Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> SyncAddress()
        {
            var provinces = await _ghnApiClient.GetProvinceAsync(apiConfig);
            var dictricts = await _ghnApiClient.GetDistrictAsync(apiConfig);
            var response = dictricts.ToList();
            foreach (var dictrict in response)
            {
                var province = provinces.FirstOrDefault(i => i.ProvinceId == dictrict.ProvinceID);
                if (province == null) continue;

                dictrict.ProvinceName = province.ProvinceName;
            }

            await JsonFileHelper.SaveDeliveryAddress<DistrictResponse>(response);

            return Ok();
        }
    }
}
