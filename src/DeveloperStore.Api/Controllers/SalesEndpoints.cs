using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.UseCases.Sales;
using MediatR;

namespace DeveloperStore.Api.Controllers
{
    public static class SalesEndpoints
    {
        public static void MapSalesEndpoints(this WebApplication app)
        {
            app.MapPost("/api/sales", async (CreateSaleDto dto, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateSaleCommand { Dto = dto });
                return Results.Created($"/api/sales/{result.SaleNumber}", result);
            }).WithTags("Sales");
        }
    }
}
