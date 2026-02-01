using Talabat.Core;
using Talabat.Core.Models.OrderValues;
using Talabat.Core.Models.Payment;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrderSpecifications;
using Talabat.Service.Interfaces;

namespace Talabat.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnumerable<IPaymentStrategy> _paymentStrategy;

        public PaymentService(IUnitOfWork unitOfWork
            , IEnumerable<IPaymentStrategy> paymentStrategy)
        {
            _unitOfWork = unitOfWork;
            _paymentStrategy = paymentStrategy;
        }
        public async Task<PaymentResponse?> CreateOrUpdatePaymentIntent(int orderId, string paymentGatewayName, string idempotencyKey)
        {
            var Order = await _unitOfWork.Repository<Order>()
                .GetByIdWithSpecificationAsync(new OrderByIdSpecification(orderId));
            if (Order is null)
                return null;
            var paymentStrategy = ResolveStrategy(paymentGatewayName);
            var paymentResponse = await paymentStrategy
                .ProcessPaymentAsync(Order, idempotencyKey);
            Order.PaymentIntentId = paymentResponse.PaymentReference;
            var response = new PaymentResponse()
            {
                OrderId = Order.Id,
                ClientSecret = paymentResponse.ClientSecret
            };
            await _unitOfWork.CompleteAsync();
            return response;
        }

        public async Task<Order> UpdatePaymentIntentToSucceededOrFailed(string PaymentIntentId, bool IsSuccessed)
        {
            var Order = await _unitOfWork.Repository<Order>()
                .GetByIdWithSpecificationAsync(new OrderByPaymentIntentSpecification(PaymentIntentId));
            Order.Status = IsSuccessed ? OrderStatus.PaymentReceived : OrderStatus.PaymentFailed;
            _unitOfWork.Repository<Order>().Update(Order);
            await _unitOfWork.CompleteAsync();
            return Order;
        }
        private IPaymentStrategy ResolveStrategy(string paymentGatewayName)
        {
            var startegy = _paymentStrategy.FirstOrDefault(s => s.Name.Equals(paymentGatewayName, StringComparison.OrdinalIgnoreCase));
            if (startegy is null)
                throw new Exception($"Payment gateway '{paymentGatewayName}' is not supported");
            return startegy;
        }
    }
}

