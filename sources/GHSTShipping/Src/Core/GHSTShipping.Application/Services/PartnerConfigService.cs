using Delivery.GHN;
using Delivery.GHN.Constants;
using Delivery.GHN.Models;
using GHSTShipping.Application.DTOs.PartnerConfig;
using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Features.Configs.Commands;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Services
{
    public class PartnerConfigService(
        IShopPartnerConfigRepository shopPartnerConfigRepository,
        IPartnerConfigRepository partnerConfigRepository,
        IGhnApiClient ghnApiClient
        , IUnitOfWork unitOfWork) : IPartnerConfigService
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

        public async Task<IEnumerable<PartnerConfigDto>> GetPartnerConfigsAsync(bool? isActivated = null)
        {
            var query = partnerConfigRepository.All();
            if (isActivated.HasValue)
            {
                query = query.Where(i => i.IsActivated == isActivated);
            }

            var result = await query.Select(i => new PartnerConfigDto()
            {
                Id = i.Id,
                ApiKey = i.ApiKey,
                DeliveryPartner = i.DeliveryPartner,
                UserName = i.UserName,
                IsActivated = i.IsActivated,
                Email = i.Email,
                PhoneNumber = i.PhoneNumber,
                FullName = i.FullName,
                ProdEnv = i.ProdEnv
            })
            .ToListAsync();

            return result;
        }

        public async Task<PartnerConfigDto> CreatePartnerConfigAsync(CreatePartnerConfigRequest request)
        {
            var partnerConfig = new Domain.Entities.PartnerConfig(new PartnerConfigDto
            {
                ApiKey = request.ApiKey,
                DeliveryPartner = request.DeliveryPartner,
                UserName = request.UserName,
            });

            if (partnerConfig.DeliveryPartner == EnumDeliveryPartner.GHN)
            {
                partnerConfig.ProdEnv = ApiEndpoints.PROD_DOMAIN;
            }

            await partnerConfigRepository.AddAsync(partnerConfig);
            await unitOfWork.SaveChangesAsync();

            return new PartnerConfigDto
            {
                Id = partnerConfig.Id,
                ApiKey = partnerConfig?.ApiKey,
                DeliveryPartner = partnerConfig.DeliveryPartner,
                IsActivated = partnerConfig.IsActivated,
                UserName = partnerConfig?.UserName,
            };
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
                existed.Email = config.Email;
                existed.PhoneNumber = config.PhoneNumber;
                existed.FullName = config.FullName;
            }

            await unitOfWork.SaveChangesAsync();

            return BaseResult.Ok();
        }

        public async Task<IEnumerable<ShopConfigDto>> GetShopConfigsAsync(Guid shopId)
        {
            var configs = await shopPartnerConfigRepository
                .Where(i => i.ShopId == shopId)
                .Select(i => new ShopConfigDto
                {
                    ShopId = i.ShopId,
                    PartnerConfigId = i.PartnerConfigId,
                    PartnerShopId = i.PartnerShopId,
                    ClientPhone = i.ClientPhone,
                })
                .ToListAsync();

            return configs;
        }

        public async Task<BaseResult> UpdateShopConfigAsync(UpdateShopDeliveryConfigRequest request)
        {
            // Get the existing config IDs for the shop
            var existedConfig = await shopPartnerConfigRepository
                .Where(i => i.ShopId == request.ShopId && request.DeliveryConfigId == i.PartnerConfigId)
                .FirstOrDefaultAsync();

            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                if (existedConfig == null)
                {
                    var shopPartnerConfig = new ShopPartnerConfig()
                    {
                        ShopId = request.ShopId,
                        PartnerShopId = request.PartnerShopId,
                        PartnerConfigId = request.DeliveryConfigId,
                        ClientPhone = request.ClientPhone,
                    };

                    await shopPartnerConfigRepository.AddAsync(shopPartnerConfig);
                }
                else
                {
                    if (request.IsConnect)
                    {
                        existedConfig.PartnerShopId = request.PartnerShopId;
                    }
                    else
                    {
                        var sqlQuery = $@"DELETE FROM ShopPartnerConfig WHERE ShopId = '{request.ShopId}' AND PartnerConfigId = '{request.DeliveryConfigId}'";
                        await shopPartnerConfigRepository.ExecuteSqlRawAsync(sqlQuery);
                    }
                }

                // Commit changes to the database
                await unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return BaseResult.Ok();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
            }

            return BaseResult.Failure();
        }

        public async Task<IEnumerable<GhnShopDetailDto>> GetGhnShopDetailDtos(PartnerConfigDto partnerConfig, string phoneNumber = null)
        {
            try
            {
                if (partnerConfig == null)
                {
                    return new List<GhnShopDetailDto>();
                }

                var apiConfig = new ApiConfig(partnerConfig.ProdEnv, partnerConfig.ApiKey);
                var response = await ghnApiClient.GetAllShopsAsync(apiConfig, new GetAllShopsRequest
                {
                    client_phone = phoneNumber,
                    limit = 100,
                    offset = 1
                });

                if (response.Code == 200)
                {
                    return response.Data.shops.Select(i => new GhnShopDetailDto
                    {
                        Id = i._id,
                        Name = i.name,
                        Phone = i.phone,
                        Address = i.address,
                        WardCode = i.ward_code,
                        DistrictId = $"{i.district_id}",
                    });
                }
            }
            catch (Exception ex)
            {
            }

            return Enumerable.Empty<GhnShopDetailDto>();
        }
    }
}
