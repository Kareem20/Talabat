using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Models;
using Talabat.Core.Repositories;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public Task<bool> DeleteBasketAsync(string basketId)
        {
            return _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);
            return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var serializedBasket = JsonSerializer.Serialize(basket);
            var isBasketCreatedOrUpdated = await _database.StringSetAsync(basket.Id, serializedBasket, TimeSpan.FromDays(1));
            return !isBasketCreatedOrUpdated ? null : await GetBasketAsync(basket.Id);
        }
    }
}
