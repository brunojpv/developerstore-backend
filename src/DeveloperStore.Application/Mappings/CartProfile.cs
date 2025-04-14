using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Application.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartResponse>().ReverseMap();
            CreateMap<CartItem, CartItemRequest>().ReverseMap();
        }
    }
}
