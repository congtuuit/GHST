using GHSTShipping.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(CodeSequence))]
    public class CodeSequence : AuditableBaseEntity
    {
        public Guid ShopId { get; set; }

        public string ShopUniqueCode { get; set; }

        public int LastOrderCode { get; set; }
    }
}
