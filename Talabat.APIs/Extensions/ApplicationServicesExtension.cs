using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service.Decorators;
using Talabat.Service.Interfaces;
using Talabat.Service.Services;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            Services.AddScoped(typeof(IPaymentIdempotancyRespository), typeof(RedisPaymentIdempotencyRepository));
            Services.AddAutoMapper(typeof(MappingProfile));
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();
                    var errorResponse = new Errors.ValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddScoped<IStripeService, StripeService>();
            // Register concrete implementation
            Services.AddScoped<StripePaymentStrategy>();
            // Register decorated IPaymentStrategy
            Services.AddScoped<IPaymentStrategy>(sp =>
            {
                var stripeStrategy = sp.GetRequiredService<StripePaymentStrategy>();
                var paymentIdempotancyRespository = sp.GetRequiredService<IPaymentIdempotancyRespository>();
                return new IdempotentPaymentStrategyDecorator(stripeStrategy, paymentIdempotancyRespository);
            });
            return Services;
        }
    }
}
