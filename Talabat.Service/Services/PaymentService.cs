using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Models.OrderValues;
using Talabat.Core.Specifications.OrderSpecifications;
using Talabat.Service.DTOs;
using Talabat.Service.Interfaces;

namespace Talabat.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration
            , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<PaymentIntentResponse?> CreateOrUpdatePaymentIntent(int OrderId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var specification = new OrderByIdSpecification(OrderId);
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
                });
                Order.PaymentIntentId = intent.Id;
                clientSecret = intent.ClientSecret;
            }
            else
            {
                var intent = await stripeService.UpdateAsync(Order.PaymentIntentId, new PaymentIntentUpdateOptions
                {
                    Amount = amount,
                });
                clientSecret = intent.ClientSecret;

            }
            await _unitOfWork.CompleteAsync();
            return new PaymentIntentResponse()
            {
                OrderId = Order.Id,
                ClientSecret = clientSecret
            };
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

