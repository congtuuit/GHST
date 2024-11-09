using GHSTShipping.Application.Features.Configs.Commands;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Application.DTOs.PartnerConfig;
using System;
using GHSTShipping.Application.DTOs.Shop;
using Delivery.GHN.Models;

namespace GHSTShipping.Application.Interfaces
{
    public interface IPartnerConfigService
    {
        Task<ApiConfig> GetApiConfigAsync(EnumDeliveryPartner enumDeliveryPartner, Guid shopId);

        Task<PartnerConfigDto> GetPartnerConfigAsync(EnumDeliveryPartner enumDeliveryPartner);

        Task<IEnumerable<PartnerConfigDto>> GetPartnerConfigsAsync(bool? isActivated = null);

        Task<PartnerConfigDto> CreatePartnerConfigAsync(CreatePartnerConfigRequest request);

        Task UpdateConfig(PartnerConfigDto update);

        Task<BaseResult> UpdateConfigsAsync(IEnumerable<UpdatePartnerConfigRequest> configs);


        /// <summary>
        /// Update shop config, link the shop to configs
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="configIds"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateShopConfigAsync(UpdateShopDeliveryConfigRequest request);

        /// <summary>
        /// Assumption 1 store connect to 1 delivery config 
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        Task<IEnumerable<ShopConfigDto>> GetShopConfigsAsync(Guid shopId);

        Task<IEnumerable<GhnShopDetailDto>> GetGhnShopDetailDtos(PartnerConfigDto partnerConfig, string phoneNumber = null);
    }
}
