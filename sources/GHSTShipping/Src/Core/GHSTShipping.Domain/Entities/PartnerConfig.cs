using GHSTShipping.Domain.Common;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    /// <summary>
    /// GHN: https://api.ghn.vn/home/docs/detail
    /// </summary>
    [Table(nameof(PartnerConfig))]
    public class PartnerConfig : AuditableBaseEntity
    {
        [MaxLength(500)]
        public string ApiKey { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }

        public string SanboxEnv { get; set; }

        public string ProdEnv { get; set; }

        public bool IsActivated { get; set; }

        public EnumDeliveryPartner DeliveryPartner { get; set; }

        public string DeliveryPartnerName
        {
            get
            {
                return DeliveryPartner.GetCode();
            }
        }

        public PartnerConfig() { }

        public PartnerConfig(PartnerConfigDto dto)
        {
            ApiKey = dto.ApiKey;
            UserName = dto.UserName;
            SanboxEnv = dto.SanboxEnv;
            ProdEnv = dto.ProdEnv;
            DeliveryPartner = dto.DeliveryPartner;
        }
    }
}
