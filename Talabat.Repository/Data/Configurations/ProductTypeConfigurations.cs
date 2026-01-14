using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Models;

namespace Talabat.Repository.Data.Configurations
{
    internal class ProductTypeConfigurations : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.Property(pt => pt.Name)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
