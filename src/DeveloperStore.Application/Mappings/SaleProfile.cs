using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Application.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<Sale, SaleResponse>();
            CreateMap<SaleItem, SaleItemResponse>();

            CreateMap<SaleRequest, Sale>();
            CreateMap<SaleItemRequest, SaleItem>();
        }
    }
}
