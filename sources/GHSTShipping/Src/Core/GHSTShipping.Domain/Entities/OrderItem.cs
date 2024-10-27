using GHSTShipping.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(OrderItem))]
    public class OrderItem : AuditableBaseEntity
    {
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string Code { get; set; } = null!;
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }

        /*public OrderCategory? Category { get; set; }
        public class OrderCategory
        {
            public int Id { get; set; } // Primary key
            public string? Level1 { get; set; }
        }*/
    }
}
