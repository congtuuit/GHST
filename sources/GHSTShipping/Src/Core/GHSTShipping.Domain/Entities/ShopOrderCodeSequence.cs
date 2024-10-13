using GHSTShipping.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(ShopOrderCodeSequence))]
    public class ShopOrderCodeSequence : AuditableBaseEntity
    {
        [MaxLength(100)]
        public string SequenceName { get; set; }

        public Guid ShopId { get; set; }
    }
}
