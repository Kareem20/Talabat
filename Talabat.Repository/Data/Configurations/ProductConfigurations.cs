using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Models;

namespace Talabat.Repository.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(product => product.ProductBrand)
                   .WithMany()
                   .HasForeignKey(product => product.ProductBrandID);

            builder.HasOne(product => product.ProductType)
                   .WithMany()
                   .HasForeignKey(product => product.ProductTypeId);

            builder.Property(product => product.Name)
                  .IsRequired()
                  .HasMaxLength(100);
            builder.Property(product => product.Description)
                  .IsRequired();
            builder.Property(product => product.PictureUrl)
                   .IsRequired();
            builder.Property(product => product.Price)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
