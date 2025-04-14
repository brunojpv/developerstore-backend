using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
