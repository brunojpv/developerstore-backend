using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.UseCases.Sales
{
    public class UpdateSaleCommand : IRequest<Unit>
    {
        public UpdateSaleDto Dto { get; set; } = default!;
    }
}
