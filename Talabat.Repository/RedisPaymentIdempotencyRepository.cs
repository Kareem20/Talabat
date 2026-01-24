using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Models;
using Talabat.Core.Repositories;

namespace Talabat.Repository
{
    public class RedisPaymentIdempotencyRepository : IPaymentIdempotancyRespository
    {
        private readonly IDatabase _database;
        public RedisPaymentIdempotencyRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<PaymentIntentResponse?> GetAsync(string key)
        {
            var data = _database.StringGet($"payment:idempotency:{key}");
            if (data.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<PaymentIntentResponse>(data!);
        }
        public async Task SetAsync(string key, PaymentIntentResponse response)
        {
            var json = JsonSerializer.Serialize(response);

            await _database.StringSetAsync(
                $"payment:idempotency:{key}",
                json,
                TimeSpan.FromHours(24)
            );
        }
        public async Task<bool> TryAcquireLockAsync(string key, TimeSpan timeout)
        {
            return await _database.StringSetAsync(
                $"payment:idempotency:lock:{key}",
                "locked",
                timeout,
                When.NotExists
            );
        }
        public Task ReleaseLockAsync(string key)
        {
            return _database.KeyDeleteAsync($"payment:idempotency:lock:{key}");
        }

    }
}
