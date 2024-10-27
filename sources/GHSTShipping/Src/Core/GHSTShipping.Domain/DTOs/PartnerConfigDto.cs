using GHSTShipping.Domain.Enums;
using System;

namespace GHSTShipping.Domain.DTOs
{
    public class PartnerConfigDto
    {
        public Guid Id { get; set; }

        public string ApiKey { get; set; }

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

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
