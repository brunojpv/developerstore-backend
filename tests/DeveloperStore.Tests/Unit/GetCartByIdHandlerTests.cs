using AutoMapper;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Application.UseCases.Carts;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using NSubstitute;

namespace DeveloperStore.Tests.Unit
{
    public class GetCartByIdHandlerTests
    {
        [Fact]
        public async Task Handle_Returns_CartDto()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
            var cartRepo = Substitute.For<ICartRepository>();

            cartRepo.GetByIdAsync(1).Returns(new Cart
            {
                Id = 1,
                UserId = 10,
                Date = DateTime.UtcNow,
                Products = new List<CartItem> { new() { ProductId = 99, Quantity = 2 } }
            });

            var handler = new GetCartByIdHandler(cartRepo, mapper);
            var result = await handler.Handle(new GetCartByIdQuery { Id = 1 }, default);

            Assert.Equal(1, result.Id);
            Assert.Single(result.Products);
        }
    }
}
