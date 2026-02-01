using Talabat.Core.Models.OrderValues;
using Talabat.Core.Models.Payment;

namespace Talabat.Core.Services
{
    public interface IPaymentStrategy
    {
        public string Name { get; }
        Task<PaymentResponse> ProcessPaymentAsync(Order order, string idempotencyKey, CancellationToken ct = default);
    }
}
