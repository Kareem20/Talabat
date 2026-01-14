using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Models.OrderValues;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrderSpecifications;

namespace Talabat.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepository
            , IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string baskedId, int deliveryMethodId, ShippingAddress address)
        {
            var Basket = await _basketRepository.GetBasketAsync(baskedId);
            var OrderItems = new List<OrderItem>();
            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    OrderItems.Add(new OrderItem(product.Id, product.Name, product.PictureUrl, item.Quantity, product.Price));
                }
            }
            var ItemsCost = OrderItems.Sum(item => item.Price * item.Quantity);
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            var Order = new Order(buyerEmail, ItemsCost, address, DeliveryMethod, OrderItems);
            await _unitOfWork.Repository<Order>().AddAsync(Order);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;
        }

        public async Task<Order?> GetOrderByIDForSepcificUserAsync(string buyerEmail, int orderId)
        {
            var orderSpecification = new OrderByIdAndBuyerSpecification(orderId, buyerEmail);
            return await _unitOfWork.Repository<Order>().GetByIdWithSpecificationAsync(orderSpecification);
        }

        public async Task<IReadOnlyCollection<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var orderSpecification = new OrdersForBuyerSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(orderSpecification);
        }
    }
}
