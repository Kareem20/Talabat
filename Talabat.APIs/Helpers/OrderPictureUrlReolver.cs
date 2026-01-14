using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Models.OrderValues;

namespace Talabat.APIs.Helpers
{
    public class OrderPictureUrlReolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderPictureUrlReolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
            {
                return string.Empty;
            }
            return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
        }
    }
}

