using GHSTShipping.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GHSTShipping.Infrastructure.Persistence.Contexts.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.BarCode).HasMaxLength(50);
        }
    }
}
