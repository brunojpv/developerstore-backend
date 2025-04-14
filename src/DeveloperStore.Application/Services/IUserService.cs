using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Services
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(int id);
        Task<UserResponse> CreateAsync(UserRequest request);
        Task UpdateAsync(int id, UserRequest request);
        Task DeleteAsync(int id);
    }
}
