using DeveloperStore.Application.Events;
using DeveloperStore.Application.UseCases.Sales;
using DeveloperStore.Infrastructure.Data;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand>
    {
        private readonly AppDbContext _context;
        private readonly IEventPublisher _eventPublisher;

        public CancelSaleCommandHandler(AppDbContext context, IEventPublisher eventPublisher)
        {
            _context = context;
            _eventPublisher = eventPublisher;
        }

        public async Task<Unit> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _context.Sales.FindAsync(request.Id);
            if (sale == null) throw new InvalidOperationException("Venda não encontrada");

            sale.Cancelled = true;
            await _context.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishAsync(new SaleCancelledEvent
            {
                SaleId = sale.Id,
                Reason = request.Reason
            });

            return Unit.Value;
        }
    }
}
