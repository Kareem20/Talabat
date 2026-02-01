using Talabat.Core.Models.OrderValues;

namespace Talabat.Core.Models.Payment
{
    public class PaymentResponse
    {
        public int OrderId { get; set; }
        // External payment reference from the selected payment gateway
        public string PaymentReference { get; set; }
        // Optional: only used for certain payment gateways
        public string? ClientSecret { get; set; }
        public OrderStatus Status { get; set; }
    }
}
