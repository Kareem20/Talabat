using Stripe;
using Talabat.Core.Models.OrderValues;
using Talabat.Core.Services;

namespace Talabat.Service.Services
{
    public class StripeService : IStripeService
    {
        public async Task<PaymentIntent> CreatePaymentIntent(decimal orderCost, string idempotencyKey)
        {
            var stripeService = new PaymentIntentService();
            long amount = (long)(orderCost * 100);
            var intent = await stripeService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
            }, new RequestOptions
            {
                IdempotencyKey = idempotencyKey
            });
            return intent;
        }

        public async Task<PaymentIntent> UpdatePaymentIntent(Order order, string idempotencyKey)
        {
            var stripeService = new PaymentIntentService();
            long amount = (long)(order.TotalCost() * 100);
            var intent = await stripeService.UpdateAsync(order.PaymentIntentId, new PaymentIntentUpdateOptions
            {
                Amount = amount,
            }, new RequestOptions
            {
                IdempotencyKey = idempotencyKey
            });
            return intent;
        }
    }
}
