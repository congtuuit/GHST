using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Services
{
    public class PartnerConfigService(IPartnerConfigRepository partnerConfigRepository) : IPartnerConfigService
    {
        public async Task<PartnerConfigDto> GetPartnerConfigAsync(EnumDeliveryPartner enumDeliveryPartner)
        {
            var result = await partnerConfigRepository.Where(i => i.DeliveryPartner == enumDeliveryPartner && i.IsActivated)
                .Select(i => new PartnerConfigDto()
                {
                    Id = i.Id,
                    ApiKey = i.ApiKey,
                    DeliveryPartner = enumDeliveryPartner,
                    UserName = i.UserName,
                    ProdEnv = i.ProdEnv,
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
