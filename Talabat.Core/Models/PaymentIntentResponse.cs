namespace Talabat.Core.Models
{
    public class PaymentIntentResponse
    {
        public int OrderId { get; set; }
        public string ClientSecret { get; set; }
    }
}
