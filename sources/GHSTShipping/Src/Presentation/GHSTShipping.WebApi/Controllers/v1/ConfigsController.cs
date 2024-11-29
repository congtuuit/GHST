using GHSTShipping.Application.DTOs.PartnerConfig;
using GHSTShipping.Application.Features.Configs.Commands;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    public class ConfigsController : BaseApiController
    {

        private readonly IPartnerConfigService _partnerConfigService;
        private readonly IOrderCodeSequenceService _orderCodeSequenceService;

        public ConfigsController(IPartnerConfigService partnerConfigService, IOrderCodeSequenceService orderCodeSequenceService)
        {
            _partnerConfigService = partnerConfigService;
            _orderCodeSequenceService = orderCodeSequenceService;
        }

        [HttpGet]
        public async Task<BaseResult<IEnumerable<PartnerConfigDto>>> Delivery()
        {
            var configs = await _partnerConfigService.GetPartnerConfigsAsync();

            return BaseResult<IEnumerable<PartnerConfigDto>>.Ok(configs);
        }

        [HttpPost]
        public async Task<BaseResult<PartnerConfigDto>> Delivery([FromBody] CreatePartnerConfigRequest request)
        {
            var configs = await _partnerConfigService.CreatePartnerConfigAsync(request);

            return BaseResult<PartnerConfigDto>.Ok(configs);
        }

        [HttpPut]
        public async Task<BaseResult> Delivery([FromBody] IEnumerable<UpdatePartnerConfigRequest> requests)
        {
            var result = await _partnerConfigService.UpdateConfigsAsync(requests);

            return result;
        }

        [HttpPut]
        public async Task<BaseResult> Shop([FromBody] UpdateShopDeliveryConfigRequest request)
        {
            var result = await _partnerConfigService.UpdateShopConfigAsync(request);

            return result;
        }
    }
}

