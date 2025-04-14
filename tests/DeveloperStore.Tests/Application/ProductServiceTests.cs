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
    public class ProductServiceTests
    {
        private readonly IProductRepository _repository = Substitute.For<IProductRepository>();
        private readonly IMapper _mapper;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
            });

            _mapper = config.CreateMapper();
            _service = new ProductService(_repository, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnProductResponse_WhenValidRequest()
        {
            // Arrange
            var request = new ProductRequest
            {
                Title = "Produto Teste",
                Price = 99.99m,
                Description = "Descrição teste",
                Category = "Eletrônicos",
                Image = "https://image.com/img.jpg",
                Rating = new RatingDto { Rate = 4.5m, Count = 20 }
            };

            var product = _mapper.Map<Product>(request);
            product.Id = 1;

            _repository.AddAsync(Arg.Any<Product>()).Returns(product);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Produto Teste", result.Title);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfProducts()
        {
            // Arrange
            var products = new Faker<Product>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Title, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
                .RuleFor(p => p.Category, f => f.Commerce.Categories(1).First())
                .Generate(3);

            _repository.GetAllAsync().Returns(products);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Title)));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
        {
            // Arrange
            var product = new Product { Id = 1, Title = "TV", Price = 2000 };
            _repository.GetByIdAsync(1).Returns(product);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TV", result!.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _repository.GetByIdAsync(100).Returns((Product?)null);

            // Act
            var result = await _service.GetByIdAsync(100);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenFound()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Title = "Old",
                Price = 100,
                Description = "Old desc",
                Category = "Old cat",
                Image = "",
                Rating = new Rating { Rate = 3.0m, Count = 10 }
            };

            var request = new ProductRequest
            {
                Title = "New Title",
                Price = 200,
                Description = "Updated desc",
                Category = "Updated",
                Image = "img.jpg",
                Rating = new RatingDto { Rate = 4.5m, Count = 22 }
            };

            _repository.GetByIdAsync(1).Returns(product);

            // Act
            await _service.UpdateAsync(1, request);

            // Assert
            Assert.Equal("New Title", product.Title);
            Assert.Equal("Updated", product.Category);
            await _repository.Received().UpdateAsync(product);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenNotFound()
        {
            // Arrange
            _repository.GetByIdAsync(99).Returns((Product?)null);

            var request = new ProductRequest
            {
                Title = "Fail",
                Price = 0,
                Description = "",
                Category = "",
                Image = "",
                Rating = new()
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

        [Fact]
        public async Task GetCategoriesAsync_ShouldReturnList()
        {
            // Arrange
            _repository.GetCategoriesAsync().Returns(new List<string> { "Eletrônicos", "Roupas" });

            // Act
            var result = await _service.GetCategoriesAsync();

            // Assert
            Assert.Contains("Roupas", result);
        }

        [Fact]
        public async Task GetByCategoryAsync_ShouldReturnProductsByCategory()
        {
            // Arrange
            var products = new Faker<Product>()
                .RuleFor(p => p.Category, _ => "Informática")
                .Generate(2);

            _repository.GetByCategoryAsync("Informática").Returns(products);

            // Act
            var result = await _service.GetByCategoryAsync("Informática");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Equal("Informática", p.Category));
        }
    }
}
