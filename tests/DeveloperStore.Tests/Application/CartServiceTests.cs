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
    public class CartServiceTests
    {
        private readonly ICartRepository _repository = Substitute.For<ICartRepository>();
        private readonly IMapper _mapper;
        private readonly CartService _service;

        public CartServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CartProfile>();
            });

            _mapper = config.CreateMapper();
            _service = new CartService(_repository, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCartResponse_WhenValid()
        {
            // Arrange
            var request = new CartRequest
            {
                UserId = 1,
                Date = DateTime.UtcNow,
                Products = new List<CartItemRequest>
            {
                new() { ProductId = 1, Quantity = 2 }
            }
            };

            var cart = _mapper.Map<Cart>(request);
            cart.Id = 1;

            _repository.AddAsync(Arg.Any<Cart>()).Returns(cart);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfCarts()
        {
            // Arrange
            var carts = new Faker<Cart>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
                .RuleFor(c => c.UserId, f => f.Random.Int(1, 10))
                .RuleFor(c => c.Date, f => f.Date.Recent())
                .RuleFor(c => c.Products, f => new List<CartItem>
                {
                new() { ProductId = 1, Quantity = 1 }
                })
                .Generate(3);

            _repository.GetAllAsync().Returns(carts);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCart_WhenFound()
        {
            // Arrange
            var cart = new Cart
            {
                Id = 1,
                UserId = 1,
                Date = DateTime.UtcNow,
                Products = new List<CartItem> { new() { ProductId = 1, Quantity = 1 } }
            };

            _repository.GetByIdAsync(1).Returns(cart);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.UserId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _repository.GetByIdAsync(99).Returns((Cart?)null);

            // Act
            var result = await _service.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCart_WhenFound()
        {
            // Arrange
            var cart = new Cart
            {
                Id = 1,
                UserId = 1,
                Date = DateTime.UtcNow,
                Products = new List<CartItem> { new() { ProductId = 1, Quantity = 1 } }
            };

            var request = new CartRequest
            {
                UserId = 2,
                Date = DateTime.UtcNow.AddDays(1),
                Products = new List<CartItemRequest> { new() { ProductId = 2, Quantity = 3 } }
            };

            _repository.GetByIdAsync(1).Returns(cart);

            // Act
            await _service.UpdateAsync(1, request);

            // Assert
            Assert.Equal(2, cart.UserId);
            Assert.Single(cart.Products);
            Assert.Equal(2, cart.Products[0].ProductId);
            await _repository.Received().UpdateAsync(cart);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenCartNotFound()
        {
            // Arrange
            _repository.GetByIdAsync(123).Returns((Cart?)null);

            var request = new CartRequest
            {
                UserId = 1,
                Date = DateTime.UtcNow,
                Products = new List<CartItemRequest>()
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(123, request));
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
