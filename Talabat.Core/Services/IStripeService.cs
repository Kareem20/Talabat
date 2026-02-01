using Talabat.Core.Models.OrderValues;

namespace Talabat.Core.Services
{
    public interface IStripeService
    {
        Task<Stripe.PaymentIntent> CreatePaymentIntent(decimal orderCost, string idempotencyKey);
        Task<Stripe.PaymentIntent> UpdatePaymentIntent(Order order, string idempotencyKey);
    }
}
