using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Models.OrderValues;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications.OrderSpecifications;
using Talabat.Service.Interfaces;

namespace Talabat.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentIdempotancyRespository _paymentIdempotancyRespository;

        public PaymentService(IConfiguration configuration
            , IUnitOfWork unitOfWork
            , IPaymentIdempotancyRespository paymentIdempotancyRespository)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _paymentIdempotancyRespository = paymentIdempotancyRespository;
        }
        public async Task<PaymentIntentResponse?> CreateOrUpdatePaymentIntent(int orderId, string idempotencyKey)
        {
            var cashedResponse = await _paymentIdempotancyRespository.GetAsync(idempotencyKey);
            if (cashedResponse is not null)
                return cashedResponse;
            var specification = new OrderByIdSpecification(orderId);
            var Order = await _unitOfWork.Repository<Order>().GetByIdWithSpecificationAsync(specification);
            if (Order is null)
                return null;
            // if (Order.Status == OrderStatus.PaymentReceived) // TODO: Exception to be handled
            var stripeService = new PaymentIntentService();
            var amount = (long)Order.TotalCost() * 100;
            var clientSecret = string.Empty;
            if (string.IsNullOrEmpty(Order.PaymentIntentId))
            {
                var intent = await stripeService.CreateAsync(new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },
                }, new RequestOptions
                {
                    IdempotencyKey = idempotencyKey
                });
                Order.PaymentIntentId = intent.Id;
                clientSecret = intent.ClientSecret;
            }
            else
            {
                var intent = await stripeService.UpdateAsync(Order.PaymentIntentId, new PaymentIntentUpdateOptions
                {
                    Amount = amount,
                }, new RequestOptions
                {
                    IdempotencyKey = idempotencyKey
                });
                clientSecret = intent.ClientSecret;

            }
            await _unitOfWork.CompleteAsync();
            var response = new PaymentIntentResponse()
            {
                OrderId = Order.Id,
                ClientSecret = clientSecret
            };
            await _paymentIdempotancyRespository.SetAsync(idempotencyKey, response);
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
    }
}

