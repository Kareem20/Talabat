using Talabat.Core.Models;

namespace Talabat.Core.Repositories
{
    public interface IPaymentIdempotancyRespository
    {
        Task<PaymentIntentResponse> GetAsync(string key);
        Task SetAsync(string key, PaymentIntentResponse intent);
    }
}
