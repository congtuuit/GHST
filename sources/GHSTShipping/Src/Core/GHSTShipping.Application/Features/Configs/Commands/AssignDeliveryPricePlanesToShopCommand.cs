using Delivery.GHN.Models;
using Delivery.GHN;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Configs.Commands
{
    public class AssignDeliveryPricePlanesToShopCommand : IRequest<bool>
    {
        public Guid ShopId { get; set; }

        public Guid DeliveryPricePlaneId { get; set; }

        public Guid DeliveryConfigId { get; set; }

        public string PartnerShopId { get; set; }

        public string ClientPhone { get; set; }

        public string Address { get; set; }

        public string WardName { get; set; }

        public string DistrictName { get; set; }

        public string ProvineName { get; set; }
    }

    public class AssignDeliveryPricePlanesToShopHandler : IRequestHandler<AssignDeliveryPricePlanesToShopCommand, bool>
    {
        private readonly IDeliveryPricePlaneRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPartnerConfigRepository _partnerConfigRepository;
        private readonly IGhnApiClient _ghnApiClient;

        public AssignDeliveryPricePlanesToShopHandler(
            IDeliveryPricePlaneRepository repository,
            IPartnerConfigRepository partnerConfigRepository,
            IGhnApiClient ghnApiClient,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _partnerConfigRepository = partnerConfigRepository;
            _ghnApiClient = ghnApiClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AssignDeliveryPricePlanesToShopCommand request, CancellationToken cancellationToken)
        {
            var deliveryPricePlaneId = await _repository
                .Where(i => request.DeliveryPricePlaneId == i.Id)
                .Select(i => i.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (deliveryPricePlaneId == Guid.Empty)
            {
                throw new KeyNotFoundException("No DeliveryPricePlanes found with the provided Ids.");
            }

            string address = request.Address;
            string wardName = request.WardName;
            string wardId = string.Empty;
            string districtName = request.DistrictName;
            string districtId = string.Empty;
            string provinceName = request.ProvineName;
            string provinceId = string.Empty;
            string shopName = string.Empty;
            string apiKey = string.Empty;
            string prodEnv = string.Empty;

            if (request.PartnerShopId != null)
            {
                var partnerConfig = await _partnerConfigRepository.Where(i => i.Id == request.DeliveryConfigId).Select(i => new
                {
                    i.DeliveryPartner,
                    i.PhoneNumber,
                    i.ApiKey,
                    i.ProdEnv,
                })
                .FirstOrDefaultAsync(cancellationToken);

                if (partnerConfig != null)
                {
                    apiKey = partnerConfig.ApiKey;
                    prodEnv = partnerConfig.ProdEnv;
                }

                if (partnerConfig != null && partnerConfig.DeliveryPartner == EnumDeliveryPartner.GHN)
                {
                    var apiConfig = new ApiConfig(partnerConfig.ProdEnv, partnerConfig.ApiKey);

                    /// Lấy thông tin của các shop thuộc token GHN
                    var shopsResult = await _ghnApiClient.GetAllShopsAsync(apiConfig, new GetAllShopsRequest
                    {
                        offset = 1,
                        limit = 200,
                        client_phone = partnerConfig.PhoneNumber,
                    });

                    if (shopsResult.Code == 200)
                    {
                        // Mapping thông tin địa chỉ
                        var shops = shopsResult.Data.shops;
                        var targetShop = shops.FirstOrDefault(i => i._id.ToString() == request.PartnerShopId);
                        if (targetShop != null)
                        {
                            address = targetShop.address;
                            shopName = targetShop.name;

                            var districts = await _ghnApiClient.GetDistrictAsync(apiConfig);
                            var targetDisctrict = districts.FirstOrDefault(i => i.DistrictID == targetShop.district_id);
                            if (targetDisctrict != null)
                            {
                                provinceName = targetDisctrict.ProvinceName;
                                provinceId = targetDisctrict.ProvinceID.ToString();
                                districtName = targetDisctrict.DistrictName;
                                districtId = targetDisctrict.DistrictID.ToString();
                            }

                            var wards = await _ghnApiClient.GetWardAsync(apiConfig, targetDisctrict.DistrictID);
                            var targetWard = wards.FirstOrDefault(i => i.WardCode == targetShop.ward_code);
                            if (targetWard != null)
                            {
                                wardId = targetWard.WardCode;
                                wardName = targetWard.WardName;
                            }
                        }
                    } else
                    {
                        throw new Exception(shopsResult.CodeMessageValue);
                    }
                }
            }

            // Tạo kết nối cùng với bảng giá
            var shopPricePlance = new DeliveryPricePlane()
            {
                ShopId = request.ShopId,
                RelatedToDeliveryPricePlaneId = deliveryPricePlaneId,
                PartnerShopId = request.PartnerShopId,
                ApiKey = apiKey,
                ProdEnv = prodEnv,
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

            await _repository.AddAsync(shopPricePlance);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
