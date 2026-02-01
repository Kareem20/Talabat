using Stripe;
using Talabat.Core.Models.OrderValues;
using Talabat.Core.Models.Payment;
using Talabat.Core.Services;

namespace Talabat.Service.Services
{
    public class StripePaymentStrategy : IPaymentStrategy
    {
        private readonly IStripeService _stripeService;

        public string Name => "Stripe";
        public StripePaymentStrategy(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }
        public async Task<PaymentResponse> ProcessPaymentAsync(Order order, string idempotencyKey, CancellationToken ct = default)
        {
            PaymentIntent intent = string.IsNullOrEmpty(order.PaymentIntentId)
                ? await _stripeService.CreatePaymentIntent(order.TotalCost(), idempotencyKey)
                : await _stripeService.UpdatePaymentIntent(order, idempotencyKey);
            return new PaymentResponse
            {
                OrderId = order.Id,
                PaymentReference = intent.Id,
                ClientSecret = intent.ClientSecret,
                Status = OrderStatus.Pending
            };
        }
    }
}
