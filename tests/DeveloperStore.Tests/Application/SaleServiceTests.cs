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
    public class SaleServiceTests
    {
        private readonly ISaleRepository _repository = Substitute.For<ISaleRepository>();
        private readonly IMapper _mapper;
        private readonly SaleService _service;

        public SaleServiceTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SaleProfile>());
            _mapper = config.CreateMapper();
            _service = new SaleService(_repository, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedSale_WhenValidRequest()
        {
            // Arrange
            var request = new SaleRequest
            {
                Customer = "João da Silva",
                Branch = "Belém - Matriz",
                Items = new List<SaleItemRequest>
            {
                new() { ProductId = 1, Quantity = 5, UnitPrice = 10.0m }
            }
            };

            Sale? createdSale = null;
            _repository.AddAsync(Arg.Do<Sale>(s => createdSale = s)).Returns(ci => createdSale!);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("João da Silva", result.Customer);
            Assert.Single(result.Items);
            Assert.True(result.TotalAmount > 0);
            Assert.Contains("S-", result.SaleNumber);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenQuantityAbove20()
        {
            // Arrange
            var request = new SaleRequest
            {
                Customer = "Maria",
                Branch = "São Paulo",
                Items = new List<SaleItemRequest>
            {
                new() { ProductId = 1, Quantity = 21, UnitPrice = 50 }
            }
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(request));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedSales()
        {
            // Arrange
            var fakeSales = new Faker<Sale>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.SaleNumber, f => $"S-{f.Random.Number(1000)}")
                .RuleFor(s => s.SaleDate, f => f.Date.Recent())
                .RuleFor(s => s.Customer, f => f.Name.FullName())
                .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                .RuleFor(s => s.Items, f => new List<SaleItem>
                {
                new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = 1,
                    Quantity = 5,
                    UnitPrice = 10,
                }
                }).Generate(3);

            _repository.GetAllAsync().Returns(fakeSales);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Customer)));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateSale_WhenValidRequest()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var existingSale = new Sale
            {
                Id = saleId,
                Customer = "Cliente Original",
                Branch = "Filial Original",
                Items = new List<SaleItem>
        {
            new() { Id = Guid.NewGuid(), ProductId = 1, Quantity = 4, UnitPrice = 10 }
        }
            };

            var request = new SaleRequest
            {
                Customer = "Cliente Atualizado",
                Branch = "Nova Filial",
                Items = new List<SaleItemRequest>
        {
            new() { ProductId = 2, Quantity = 6, UnitPrice = 15 }
        }
            };

            _repository.GetByIdAsync(saleId).Returns(existingSale);

            // Act
            await _service.UpdateAsync(saleId, request);

            // Assert
            Assert.Equal("Cliente Atualizado", existingSale.Customer);
            Assert.Equal("Nova Filial", existingSale.Branch);
            Assert.Single(existingSale.Items);
            Assert.Equal(2, existingSale.Items[0].ProductId);
            Assert.Equal(6, existingSale.Items[0].Quantity);
            await _repository.Received().UpdateAsync(existingSale);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenSaleNotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            _repository.GetByIdAsync(nonExistentId).Returns((Sale?)null);

            var request = new SaleRequest
            {
                Customer = "Novo Cliente",
                Branch = "Nova Filial",
                Items = new List<SaleItemRequest>()
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(nonExistentId, request));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenItemExceedsMaxQuantity()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var existingSale = new Sale
            {
                Id = saleId,
                Customer = "Fulano",
                Branch = "Unidade",
                Items = new List<SaleItem>()
            };

            var request = new SaleRequest
            {
                Customer = "Fulano",
                Branch = "Unidade",
                Items = new List<SaleItemRequest>
        {
            new() { ProductId = 1, Quantity = 25, UnitPrice = 10 }
        }
            };

            _repository.GetByIdAsync(saleId).Returns(existingSale);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(saleId, request));
        }

        [Fact]
        public async Task CancelAsync_ShouldSetIsCancelled_WhenSaleExists()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var existingSale = new Sale
            {
                Id = saleId,
                Customer = "Carlos",
                Branch = "Loja Norte",
                Items = new List<SaleItem>(),
            };

            _repository.GetByIdAsync(saleId).Returns(existingSale);

            // A lógica real de cancelamento está no repositório, então vamos simular que ela será chamada.
            _repository
                .When(x => x.CancelAsync(saleId))
                .Do(_ => existingSale.Cancel());

            // Act
            await _service.CancelAsync(saleId);

            // Assert
            Assert.True(existingSale.IsCancelled);
            await _repository.Received().CancelAsync(saleId);
        }

        [Fact]
        public async Task CancelAsync_ShouldDoNothing_WhenSaleNotExists()
        {
            // Arrange
            var fakeId = Guid.NewGuid();

            _repository.GetByIdAsync(fakeId).Returns((Sale?)null);

            // Como o método não lança exceção quando não encontra, apenas certificamos que ele foi chamado corretamente
            await _service.CancelAsync(fakeId);

            // Assert
            await _repository.Received().CancelAsync(fakeId);
        }

    }
}
