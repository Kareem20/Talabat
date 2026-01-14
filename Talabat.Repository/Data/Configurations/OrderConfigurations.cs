using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Models.OrderValues;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(order => order.ShippingAdress, a =>
            {
                a.WithOwner();
            });
            builder.Property(order => order.Status)
                   .HasConversion(
                       s => s.ToString(),
                       s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s));
            builder.Property(builder => builder.ItemsCost)
                   .HasColumnType("decimal(18,2)");
            builder.HasOne(order => order.DeliveryMethod)
                   .WithMany()
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
