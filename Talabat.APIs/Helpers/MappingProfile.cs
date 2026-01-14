using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Models;
using Talabat.Core.Models.OrderValues;

namespace Talabat.APIs.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(Productdto => Productdto.ProductBrand
                    , options => options.MapFrom(product => product.ProductBrand.Name))
                .ForMember(Productdto => Productdto.ProductType
                    , options => options.MapFrom(product => product.ProductType.Name))
                .ForMember(Productdto => Productdto.PictureUrl
                    , options => options.MapFrom<ProductPictureUrlResolver>());
            CreateMap<AddressDTO, ShippingAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(orderToReturnDto => orderToReturnDto.DeliveryMethod
                    , options => options.MapFrom(order => order.DeliveryMethod.ShortName))
                .ForMember(orderToReturnDto => orderToReturnDto.DeliveryMethodCost
                    , options => options.MapFrom(order => order.DeliveryMethod.Cost))
                .ForMember(orderToReturnDto => orderToReturnDto.Total
                    , options => options.MapFrom(order => order.TotalCost()));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(orderItemDTO => orderItemDTO.PictureUrl
                    , options => options.MapFrom<OrderPictureUrlReolver>());
        }
    }
}
