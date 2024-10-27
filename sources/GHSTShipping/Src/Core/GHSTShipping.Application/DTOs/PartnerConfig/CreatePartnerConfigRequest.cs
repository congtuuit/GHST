using GHSTShipping.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHSTShipping.Application.DTOs.PartnerConfig
{
    public class CreatePartnerConfigRequest
    {
        public string ApiKey { get; set; }

        public string UserName { get; set; }

        public EnumDeliveryPartner DeliveryPartner { get; set; }
    }
}
