using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Sale, SaleResponseDto>();
            CreateMap<SaleItem, SaleItemResponseDto>();
            CreateMap<SaleItem, SaleItemResultDto>();

            CreateMap<CartItem, CartItemDto>();

            CreateMap<Cart, CartDto>();
        }
    }
}
