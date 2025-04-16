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

            app.MapPut("/api/sales/{id:int}", async (int id, UpdateSaleDto dto, IMediator mediator) =>
            {
                if (id != dto.Id) return Results.BadRequest("Id mismatch");
                await mediator.Send(new UpdateSaleCommand { Dto = dto });
                return Results.NoContent();
            }).WithTags("Sales");

            app.MapPut("/api/sales/{id:int}/cancel", async (int id, string reason, IMediator mediator) =>
            {
                await mediator.Send(new CancelSaleCommand { Id = id, Reason = reason });
                return Results.NoContent();
            }).WithTags("Sales");

            app.MapGet("/api/sales/{id:int}", (int id) =>
            {
                return Results.Ok(new { message = $"Venda {id} simulada." });
            }).WithTags("Sales");
        }
    }
}
