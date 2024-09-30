using AutoMapper;
using Delivery.GHN.Models;
using GHSTShipping.Application.Features.Orders.Commands;

namespace GHSTShipping.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Define the mapping between CreateGhnOrderRequest and CreateDeliveryOrderRequest
            CreateMap<CreateGhnOrderRequest, CreateDeliveryOrderRequest>();
        }
    }
}
