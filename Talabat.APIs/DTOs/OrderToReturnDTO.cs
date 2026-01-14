using Talabat.Core.Models.OrderValues;

namespace Talabat.APIs.DTOs
{
    public class OrderToReturnDTO
    {
        public int ID { get; set; }
        public string BuyerEmail { get; set; }
        public string PaymentIntentID { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Status { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public decimal ItemsCost { get; set; }
        public decimal Total { get; set; }
        public ShippingAddress ShippingAdress { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; } = new HashSet<OrderItemDTO>();


    }
}
