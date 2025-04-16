using AutoMapper;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Application.UseCases.Products;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using NSubstitute;

namespace DeveloperStore.Tests.Unit
{
    public class GetAllProductsHandlerTests
    {
        [Fact]
        public async Task Handle_Returns_ProductDtos()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
            var productRepo = Substitute.For<IProductRepository>();

            productRepo.GetAllAsync(Arg.Any<string>()).Returns(new List<Product>
            {
                new() { Id = 1, Title = "Notebook", Price = 3000, Category = "Eletrônicos" }
            });

            var handler = new GetAllProductsHandler(productRepo, mapper);
            var result = await handler.Handle(new GetAllProductsQuery { Filter = null }, default);

            Assert.Single(result);
            Assert.Equal("Notebook", result.First().Title);
        }
    }
}
