using Talabat.Core.Models.OrderValues;
using Talabat.Service.DTOs;

namespace Talabat.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentIntentResponse?> CreateOrUpdatePaymentIntent(int OrderID);
        Task<Order> UpdatePaymentIntentToSucceededOrFailed(string PaymentIntentId, bool IsSuccessed);
    }
}
