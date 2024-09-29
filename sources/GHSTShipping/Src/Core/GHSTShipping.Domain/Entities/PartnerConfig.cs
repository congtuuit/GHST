using GHSTShipping.Domain.Common;
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

        [MaxLength(100)]
        public string PartnerName { get; set; }
    }
}
