using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FocusSpace.Models;

namespace FocusSpace.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.SKU).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Category).IsRequired().HasMaxLength(50);
            builder.HasIndex(p => p.SKU).IsUnique();
        }
    }
}
