using AutoMapper;
using GHSTShipping.Application.Features.Orders.Commands;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Entities;

namespace GHSTShipping.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Define the mapping between CreateGhnOrderRequest and CreateDeliveryOrderRequest
            CreateMap<GHN_CreateOrderRequest, CreateDeliveryOrderRequest>();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.ShopName, opt => opt.MapFrom(i => i.Shop.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(i => i.CurrentStatus))
                ;

            CreateMap<Order, OrderDetailDto>()
               .ForMember(dest => dest.ShopName, opt => opt.MapFrom(i => i.Shop.Name))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(i => i.CurrentStatus))
               .ForMember(dest => dest.PrivateOrderCode, opt => opt.MapFrom(i => i.private_order_code))
               ;

            CreateMap<Order, CreateDeliveryOrderRequest>();
            CreateMap<OrderItem, CreateDeliveryOrderRequest.Item>();
        }
    }
}
