namespace Talabat.Core.Models.OrderValues
{
    public class Order : BaseEntity
    {
        public string BuyerEmail { get; set; }
        public decimal ItemsCost { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ShippingAddress ShippingAdress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public Order()
        {

        }
        public Order(string buyerEmail, decimal itemsCost, ShippingAddress shippingAdress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items)
        {
            BuyerEmail = buyerEmail;
            ItemsCost = itemsCost;
            ShippingAdress = shippingAdress;
            DeliveryMethod = deliveryMethod;
            Items = items;
        }
        public decimal TotalCost()
        {
            return ItemsCost + DeliveryMethod.Cost;
        }
    }
}
