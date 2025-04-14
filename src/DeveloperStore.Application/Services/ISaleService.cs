using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Services
{
    public interface ISaleService
    {
        Task<SaleResponse> CreateAsync(SaleRequest request);
        Task<List<SaleResponse>> GetAllAsync();
        Task<SaleResponse?> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, SaleRequest request);
        Task CancelAsync(Guid id);
    }
}
