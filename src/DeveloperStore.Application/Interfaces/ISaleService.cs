using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Interfaces
{
    public interface ISaleService
    {
        Task<SaleResponseDto> CreateSaleAsync(CreateSaleDto dto);
    }
}
