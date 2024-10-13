using GHSTShipping.Application.Features.Configs.Commands;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using GHSTShipping.Application.Wrappers;

namespace GHSTShipping.Application.Interfaces
{
    public interface IPartnerConfigService
    {
        Task<PartnerConfigDto> GetPartnerConfigAsync(EnumDeliveryPartner enumDeliveryPartner);

        Task<IEnumerable<PartnerConfigDto>> GetPartnerConfigsAsync();

        Task UpdateConfig(PartnerConfigDto update);

        Task<BaseResult> UpdateConfigsAsync(IEnumerable<UpdatePartnerConfigRequest> configs);
    }
}
