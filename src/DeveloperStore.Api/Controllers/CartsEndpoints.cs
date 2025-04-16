using DeveloperStore.Application.UseCases.Carts;
using MediatR;

namespace DeveloperStore.Api.Controllers
{
    public static class CartsEndpoints
    {
        public static void MapCartsEndpoints(this WebApplication app)
        {
            app.MapGet("/api/carts/{id:int}", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCartByIdQuery { Id = id });
                return Results.Ok(result);
            }).WithTags("Carts");
        }
    }
}
