using MediatR;

namespace DeveloperStore.Application.UseCases.Sales
{
    public class CancelSaleCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
