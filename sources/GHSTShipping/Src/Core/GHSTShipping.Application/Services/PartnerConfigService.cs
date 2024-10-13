using GHSTShipping.Application.Features.Configs.Commands;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Services
{
    public class PartnerConfigService(IPartnerConfigRepository partnerConfigRepository, IUnitOfWork unitOfWork) : IPartnerConfigService
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

        public async Task<IEnumerable<PartnerConfigDto>> GetPartnerConfigsAsync()
        {
            var result = await partnerConfigRepository.All()
                .Select(i => new PartnerConfigDto()
                {
                    Id = i.Id,
                    ApiKey = i.ApiKey,
                    DeliveryPartner = i.DeliveryPartner,
                    UserName = i.UserName,
                    ProdEnv = i.ProdEnv,
                })
                .ToListAsync();

            return result;
        }

        public async Task UpdateConfig(PartnerConfigDto update)
        {
            var result = await partnerConfigRepository.Where(i => i.DeliveryPartner == update.DeliveryPartner)
               .Select(i => new Domain.Entities.PartnerConfig()
               {
                   Id = i.Id,
               })
               .FirstOrDefaultAsync();

            if (result != null)
            {
                partnerConfigRepository.Modify(result);
                result.ApiKey = update.ApiKey;
                result.IsActivated = update.IsActivated;

                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<BaseResult> UpdateConfigsAsync(IEnumerable<UpdatePartnerConfigRequest> configs)
        {
            var _configs = await partnerConfigRepository.Where(i => configs.Select(c => c.Id).Contains(i.Id))
               .ToListAsync();

            partnerConfigRepository.ModifyRange(_configs);
            foreach (var config in configs)
            {
                var existed = _configs.FirstOrDefault(i => i.Id == config.Id);
                if (existed == null) continue;

                existed.ApiKey = config.ApiKey;
                existed.UserName = config.UserName;
                existed.IsActivated = config.IsActivated;
            }

            await unitOfWork.SaveChangesAsync();

            return BaseResult.Ok();
        }
    }
}
