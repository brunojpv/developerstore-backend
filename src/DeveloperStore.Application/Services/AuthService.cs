using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;

        public AuthService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var users = await _repository.GetAllAsync();

            var user = users.FirstOrDefault(u =>
                u.Username == request.Username &&
                u.Password == request.Password);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid username or password");

            return new LoginResponse
            {
                Token = $"fake-token-{Guid.NewGuid()}"
            };
        }
    }
}
