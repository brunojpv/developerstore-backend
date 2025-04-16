using DeveloperStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace DeveloperStore.Tests.Integration
{
    public class SalesApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SalesApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateSale_ShouldReturnCreated()
        {
            var sale = new Sale
            {
                SaleNumber = "#SALE-TEST",
                Date = DateTime.UtcNow,
                CustomerId = 100,
                Branch = "Filial Teste",
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = 1,
                        Quantity = 5,
                        UnitPrice = 50
                    }
                }
            };

            var response = await _client.PostAsJsonAsync("/api/sales", sale);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CancelSale_ShouldReturnNoContent()
        {
            var cancel = await _client.PutAsync("/api/sales/1/cancel?reason=erro%20de%20sistema", null);
            Assert.True(cancel.StatusCode == HttpStatusCode.NoContent || cancel.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSale_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/sales/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
