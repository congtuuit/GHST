using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Enums;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces
{
    public interface IPartnerConfigService
    {
        Task<PartnerConfigDto> GetPartnerConfigAsync(EnumDeliveryPartner enumDeliveryPartner);
    }
}
