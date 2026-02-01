using Talabat.Core.Models.Payment;

namespace Talabat.Core.Repositories
{
    public interface IPaymentIdempotancyRespository
    {
        Task<PaymentResponse> GetAsync(string key);
        Task SetAsync(string key, PaymentResponse intent);
        Task<bool> TryAcquireLockAsync(string key, TimeSpan timeout);
        Task ReleaseLockAsync(string key);
    }
}
