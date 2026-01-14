using Talabat.Core.Models.OrderValues;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        public Task<Order?> CreateOrderAsync(string buyerEmail, string baskedId
            , int deliveryMethodId, ShippingAddress address);
        public Task<IReadOnlyCollection<Order>> GetOrdersForSpecificUserAsync(string buyerEmail);
        public Task<Order?> GetOrderByIDForSepcificUserAsync(string buyerEmail, int orderId);
    }
}
