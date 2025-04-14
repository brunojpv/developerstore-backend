using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Services
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllAsync();
        Task<ProductResponse?> GetByIdAsync(int id);
        Task<ProductResponse> CreateAsync(ProductRequest request);
        Task UpdateAsync(int id, ProductRequest request);
        Task DeleteAsync(int id);
        Task<List<string>> GetCategoriesAsync();
        Task<List<ProductResponse>> GetByCategoryAsync(string category);
    }
}
