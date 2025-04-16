using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Application.UseCases.Sales;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using NSubstitute;

namespace DeveloperStore.Tests.Unit
{
    public class CreateSaleHandlerTests
    {
        [Fact]
        public async Task CreateSale_Should_CalculateTotalCorrectly()
        {
            // Arrange
            var saleRepo = Substitute.For<ISaleRepository>();
            var productRepo = Substitute.For<IProductRepository>();

            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            productRepo.GetByIdAsync(1).Returns(new Product
            {
                Id = 1,
                Title = "Product A",
                Price = 100
            });

            var handler = new CreateSaleHandler(saleRepo, productRepo, mapper);
            var command = new CreateSaleCommand
            {
                Dto = new CreateSaleDto
                {
                    CustomerId = 1,
                    Branch = "Main",
                    Items = new List<SaleItemDto>
                {
                    new() { ProductId = 1, Quantity = 5 }
                }
                }
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(100 * 5 * 0.9m, result.TotalAmount); // 10% de desconto
        }
    }
}
