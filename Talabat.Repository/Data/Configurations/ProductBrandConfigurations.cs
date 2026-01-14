using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Models;

namespace Talabat.Repository.Data.Configurations
{
    internal class ProductBrandConfigurations : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.Property(pb => pb.Name)
                  .IsRequired()
                  .HasMaxLength(100);
        }
    }
}
