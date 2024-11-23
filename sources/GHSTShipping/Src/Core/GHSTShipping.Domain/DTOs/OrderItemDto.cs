using System;

namespace GHSTShipping.Domain.DTOs
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }

        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public int? Weight { get; set; }

        public string Code { get; set; } = null!;
        public int? Price { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
