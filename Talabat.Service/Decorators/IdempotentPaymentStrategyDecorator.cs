using Talabat.Core.Models.OrderValues;
using Talabat.Core.Models.Payment;
using Talabat.Core.Repositories;
using Talabat.Core.Services;

namespace Talabat.Service.Decorators
{
    public class IdempotentPaymentStrategyDecorator : IPaymentStrategy
    {
        private readonly IPaymentStrategy _inner;
        private readonly IPaymentIdempotancyRespository _paymentIdempotancyRespository;
        public string Name => _inner.Name;
        public IdempotentPaymentStrategyDecorator(IPaymentStrategy inner
            , IPaymentIdempotancyRespository paymentIdempotancyRespository)
        {
            _inner = inner;
            _paymentIdempotancyRespository = paymentIdempotancyRespository;
        }
        public async Task<PaymentResponse> ProcessPaymentAsync(Order order, string idempotencyKey, CancellationToken ct = default)
        {
            var cashedResponse = await _paymentIdempotancyRespository.GetAsync(idempotencyKey);
            if (cashedResponse is not null)
                return cashedResponse;
            var lockAcquired = await _paymentIdempotancyRespository
               .TryAcquireLockAsync(idempotencyKey, TimeSpan.FromSeconds(30));
            if (!lockAcquired)
                throw new Exception("Payment is already being processed");
            try
            {
                cashedResponse = await _paymentIdempotancyRespository.GetAsync(idempotencyKey);
                if (cashedResponse is not null)
                    return cashedResponse;
                var paymentResponse = await _inner.ProcessPaymentAsync(order, idempotencyKey, ct);
                await _paymentIdempotancyRespository.SetAsync(idempotencyKey, paymentResponse);
                return paymentResponse;
            }
            finally
            {
                    await _paymentIdempotancyRespository.ReleaseLockAsync(idempotencyKey);
            }
        }
    }
}
