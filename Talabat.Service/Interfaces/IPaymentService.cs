using Talabat.Core.Models.OrderValues;
using Talabat.Core.Models.Payment;

namespace Talabat.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse?> CreateOrUpdatePaymentIntent(int orderId, string paymentGatewayName, string idempotencyKey);
        Task<Order> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSuccessed);
    }
}
