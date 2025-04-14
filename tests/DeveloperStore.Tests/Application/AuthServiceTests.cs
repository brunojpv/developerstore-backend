using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Services;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using NSubstitute;

namespace DeveloperStore.Tests.Application
{
    public class AuthServiceTests
    {
        private readonly IUserRepository _repository = Substitute.For<IUserRepository>();
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _service = new AuthService(_repository);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var users = new List<User>
        {
            new() { Id = 1, Username = "bruno", Password = "1234" }
        };

            _repository.GetAllAsync().Returns(users);

            var request = new LoginRequest
            {
                Username = "bruno",
                Password = "1234"
            };

            // Act
            var result = await _service.LoginAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.StartsWith("fake-token-", result.Token);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenCredentialsAreInvalid()
        {
            // Arrange
            var users = new List<User>
        {
            new() { Id = 1, Username = "admin", Password = "admin" }
        };

            _repository.GetAllAsync().Returns(users);

            var request = new LoginRequest
            {
                Username = "hacker",
                Password = "wrong"
            };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(request));
        }
    }
}
