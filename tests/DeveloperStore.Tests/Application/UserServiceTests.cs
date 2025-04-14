using AutoMapper;
using Bogus;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Application.Services;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using NSubstitute;

namespace DeveloperStore.Tests.Application
{
    public class UserServiceTests
    {
        private readonly IUserRepository _repository = Substitute.For<IUserRepository>();
        private readonly IMapper _mapper;
        private readonly UserService _service;

        public UserServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });

            _mapper = config.CreateMapper();
            _service = new UserService(_repository, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnUser_WhenValid()
        {
            // Arrange
            var request = new UserRequest
            {
                Email = "test@email.com",
                Username = "testuser",
                Password = "secret",
                Name = new NameDto { Firstname = "John", Lastname = "Doe" },
                Address = new AddressDto
                {
                    City = "SP",
                    Street = "Av. Brasil",
                    Number = 123,
                    Zipcode = "12345-000",
                    Geolocation = new GeolocationDto { Lat = "-1.0", Long = "45.0" }
                },
                Phone = "99999-9999",
                Status = "Active",
                Role = "Customer"
            };

            var createdUser = _mapper.Map<User>(request);
            createdUser.Id = 1;

            _repository.AddAsync(Arg.Any<User>()).Returns(createdUser);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("testuser", result.Username);
            await _repository.Received(1).AddAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfUsers()
        {
            // Arrange
            var users = new Faker<User>()
                .RuleFor(u => u.Id, f => f.IndexFaker + 1)
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .Generate(3);

            _repository.GetAllAsync().Returns(users);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Username)));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
        {
            // Arrange
            var user = new User { Id = 1, Username = "admin", Email = "admin@site.com" };
            _repository.GetByIdAsync(1).Returns(user);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("admin", result!.Username);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _repository.GetByIdAsync(99).Returns((User?)null);

            // Act
            var result = await _service.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser_WhenExists()
        {
            // Arrange
            var existingUser = new User
            {
                Id = 1,
                Username = "olduser",
                Email = "old@email.com",
                Name = new Name { Firstname = "Old", Lastname = "Name" },
                Address = new Address { City = "Old City", Street = "Old St", Number = 1, Zipcode = "00000-000", Geolocation = new Geolocation { Lat = "0", Long = "0" } },
                Phone = "0000",
                Status = "Inactive",
                Role = "Customer"
            };

            var request = new UserRequest
            {
                Username = "newuser",
                Email = "new@email.com",
                Password = "newpass",
                Name = new NameDto { Firstname = "New", Lastname = "Name" },
                Address = new AddressDto
                {
                    City = "New City",
                    Street = "New St",
                    Number = 123,
                    Zipcode = "11111-111",
                    Geolocation = new GeolocationDto { Lat = "1", Long = "1" }
                },
                Phone = "1111",
                Status = "Active",
                Role = "Manager"
            };

            _repository.GetByIdAsync(1).Returns(existingUser);

            // Act
            await _service.UpdateAsync(1, request);

            // Assert
            Assert.Equal("newuser", existingUser.Username);
            Assert.Equal("New City", existingUser.Address.City);
            await _repository.Received().UpdateAsync(existingUser);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            _repository.GetByIdAsync(99).Returns((User?)null);

            var request = new UserRequest
            {
                Email = "notfound@email.com",
                Username = "nf",
                Password = "x",
                Name = new(),
                Address = new(),
                Phone = "",
                Status = "Inactive",
                Role = "Customer"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(99, request));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepository()
        {
            // Act
            await _service.DeleteAsync(1);

            // Assert
            await _repository.Received().DeleteAsync(1);
        }
    }
}
