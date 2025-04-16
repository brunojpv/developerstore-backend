using DeveloperStore.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace DeveloperStore.Tests.Integration
{
    public class SalesApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SalesApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_Sale_Should_Return_201()
        {
            // Arrange
            var sale = new CreateSaleDto
            {
                CustomerId = 1,
                Branch = "Branch A",
                Items = new List<SaleItemDto>
            {
                new() { ProductId = 1, Quantity = 4 }
            }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/sales", sale);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }
    }
}
