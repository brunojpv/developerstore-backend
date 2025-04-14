using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Services
{
    public interface ICartService
    {
        Task<List<CartResponse>> GetAllAsync();
        Task<CartResponse?> GetByIdAsync(int id);
        Task<CartResponse> CreateAsync(CartRequest request);
        Task UpdateAsync(int id, CartRequest request);
        Task DeleteAsync(int id);
    }
}
