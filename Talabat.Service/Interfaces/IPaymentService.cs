using Talabat.Core.Models;
using Talabat.Core.Models.OrderValues;

namespace Talabat.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentIntentResponse?> CreateOrUpdatePaymentIntent(int orderId, string idempotencyKey);
        Task<Order> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSuccessed);
    }
}
