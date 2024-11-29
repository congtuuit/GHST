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
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        IGhnApiClient ghnApiClient
        , IUnitOfWork unitOfWork) : IPartnerConfigService
    {

        public async Task<ApiConfig> GetApiConfigAsync(EnumDeliveryPartner enumDeliveryPartner, Guid shopId)
        {
            var partnerConfig = await shopPartnerConfigRepository
                .Where(i => i.ShopId == shopId && i.PartnerConfig.DeliveryPartner == enumDeliveryPartner)
                .Select(i => new
                {
                    i.PartnerConfig.ProdEnv,
                    i.PartnerConfig.ApiKey,
                    i.PartnerShopId
                })
                .FirstOrDefaultAsync();

            if (partnerConfig == null)
            {
                return null;
            }

            var apiConfig = new ApiConfig(partnerConfig.ProdEnv, partnerConfig.ApiKey, partnerConfig.PartnerShopId);

            return apiConfig;
        }

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
                ProdEnv = i.ProdEnv,
            })
            .ToListAsync();

            var configIds = result.Select(c => c.Id);
            var connects = await shopPartnerConfigRepository.Where(c => configIds.Contains(c.PartnerConfigId))
                .Select(c => new
                {
                    c.PartnerConfigId,
                    c.ShopName,
                })
                .ToListAsync();

            foreach (var config in result)
            {
                var shopConnects = connects.Where(c => c.PartnerConfigId == config.Id);
                config.ShopNames = string.Join(", ", shopConnects.Select(c => c.ShopName));
            }

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

        /// <summary>
        /// Assumption 1 store connect to 1 delivery config 
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ShopConfigDto>> GetShopConfigsAsync(Guid shopId)
        {
            var configs = await shopPartnerConfigRepository
                .Where(i => i.ShopId == shopId)
                .Select(i => new ShopConfigDto
                {
                    ShopId = i.ShopId,
                    PartnerConfigId = i.PartnerConfigId,
                    DeliveryPartner = i.PartnerConfig.DeliveryPartner,
                    PartnerShopId = i.PartnerShopId,
                    ClientPhone = i.ClientPhone,
                    Address = i.Address,
                    WardName = i.WardName,
                    DistrictName = i.DistrictName,
                    ProvinceName = i.ProvinceName,
                })
                .ToListAsync();

            return configs;
        }

        public async Task<BaseResult> UpdateShopConfigAsync(UpdateShopDeliveryConfigRequest request)
        {
            // Get the existing config IDs for the shop
            var existedConfig = await shopPartnerConfigRepository
                .Where(i => i.ShopId == request.ShopId)
                .ToListAsync();

            string address = request.Address;
            string wardName = request.WardName;
            string wardId = string.Empty;
            string districtName = request.DistrictName;
            string districtId = string.Empty;
            string provinceName = request.ProviceName;
            string provinceId = string.Empty;
            string shopName = string.Empty;

            if (!string.IsNullOrWhiteSpace(request.PartnerShopId))
            {
                var partnerConfig = await partnerConfigRepository.Where(i => i.Id == request.DeliveryConfigId).Select(i => new
                {
                    i.DeliveryPartner,
                    i.ApiKey,
                    i.ProdEnv,
                })
                .FirstOrDefaultAsync();

                if (partnerConfig != null && partnerConfig.DeliveryPartner == EnumDeliveryPartner.GHN)
                {
                    var apiConfig = new ApiConfig(partnerConfig.ProdEnv, partnerConfig.ApiKey);
                    var shopsResult = await ghnApiClient.GetAllShopsAsync(apiConfig, new GetAllShopsRequest
                    {
                        offset = 1,
                        limit = 200,
                        client_phone = request.ClientPhone,
                    });

                    if (shopsResult.Code == 200)
                    {
                        var shops = shopsResult.Data.shops;
                        var targetShop = shops.FirstOrDefault(i => i._id.ToString() == request.PartnerShopId);
                        if (targetShop != null)
                        {
                            address = targetShop.address;
                            shopName = targetShop.name;

                            var districts = await ghnApiClient.GetDistrictAsync(apiConfig);
                            var targetDisctrict = districts.FirstOrDefault(i => i.DistrictID == targetShop.district_id);
                            if (targetDisctrict != null)
                            {
                                provinceName = targetDisctrict.ProvinceName;
                                provinceId = targetDisctrict.ProvinceID.ToString();
                                districtName = targetDisctrict.DistrictName;
                                districtId = targetDisctrict.DistrictID.ToString();
                            }

                            var wards = await ghnApiClient.GetWardAsync(apiConfig, targetDisctrict.DistrictID);
                            var targetWard = wards.FirstOrDefault(i => i.WardCode == targetShop.ward_code);
                            if (targetWard != null)
                            {
                                wardId = targetWard.WardCode;
                                wardName = targetWard.WardName;
                            }
                        }
                    }
                }
            }

            var shopPartnerConfig = new ShopPartnerConfig()
            {
                ShopId = request.ShopId,
                PartnerShopId = request.PartnerShopId,
                PartnerConfigId = request.DeliveryConfigId,
                ClientPhone = request.ClientPhone,
                ShopName = shopName,

                Address = address,
                WardName = wardName,
                WardCode = wardId,
                DistrictName = districtName,
                DistrictId = districtId,
                ProvinceName = provinceName,
                ProvinceId = provinceId,
            };

            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                // Remove all configs
                if (existedConfig.Any())
                {
                    var sqlQuery = $@"DELETE FROM ShopPartnerConfig WHERE ShopId = '{request.ShopId}'";
                    await unitOfWork.ExecuteSqlRawAsync(sqlQuery);

                    var sqlQueryDeliveryPricePlane = $@"DELETE FROM DeliveryPricePlane WHERE ShopId = '{request.ShopId}'";
                    await unitOfWork.ExecuteSqlRawAsync(sqlQueryDeliveryPricePlane);

                    // Remove all order and order item related to shop
                    var shopOrders = await orderRepository
                        .Where(i => i.LastSyncDate.HasValue)
                        .ToListAsync();

                    if (shopOrders.Count > 0)
                    {
                        orderRepository.HardDeleteRange(shopOrders);
                    }

                    var orderIds = shopOrders.Select(o => o.Id);
                    var orderItems = await orderItemRepository
                        .Where(i => orderIds.Contains(i.OrderId))
                        .ToListAsync();

                    if (orderItems.Count > 0)
                    {
                        orderItemRepository.HardDeleteRange(orderItems);
                    }
                }

                if (request.IsConnect)
                {
                    await shopPartnerConfigRepository.AddAsync(shopPartnerConfig);
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
                        Id = $"{i._id}",
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
