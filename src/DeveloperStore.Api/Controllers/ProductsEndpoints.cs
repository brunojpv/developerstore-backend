using DeveloperStore.Application.UseCases.Products;
using MediatR;

namespace DeveloperStore.Api.Controllers
{
    public static class ProductsEndpoints
    {
        public static void MapProductsEndpoints(this WebApplication app)
        {
            app.MapGet("/api/products", async (string? filter, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllProductsQuery { Filter = filter });
                return Results.Ok(result);
            }).WithTags("Products");
        }
    }
}
