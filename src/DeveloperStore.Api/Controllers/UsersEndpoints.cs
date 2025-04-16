using DeveloperStore.Application.UseCases.Users;
using MediatR;

namespace DeveloperStore.Api.Controllers
{
    public static class UsersEndpoints
    {
        public static void MapUsersEndpoints(this WebApplication app)
        {
            app.MapGet("/api/users", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllUsersQuery());
                return Results.Ok(result);
            }).WithTags("Users");
        }
    }
}
