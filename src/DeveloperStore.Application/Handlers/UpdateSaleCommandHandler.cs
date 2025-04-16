using DeveloperStore.Application.Events;
using DeveloperStore.Application.UseCases.Sales;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Infrastructure.Data;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand>
    {
        private readonly AppDbContext _context;
        private readonly IEventPublisher _eventPublisher;

        public UpdateSaleCommandHandler(AppDbContext context, IEventPublisher eventPublisher)
        {
            _context = context;
            _eventPublisher = eventPublisher;
        }

        public async Task<Unit> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var sale = await _context.Sales.FindAsync(dto.Id);
            if (sale == null) throw new InvalidOperationException("Venda não encontrada");

            sale.SaleNumber = dto.SaleNumber;
            sale.Date = dto.Date;
            sale.CustomerId = dto.CustomerId;
            sale.Branch = dto.Branch;
            sale.Cancelled = dto.Cancelled;
            sale.Items = dto.Items.Select(i => new SaleItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            await _context.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishAsync(new SaleModifiedEvent
            {
                SaleId = sale.Id,
                ModifiedBy = "api-user",
                ModifiedAt = DateTime.UtcNow
            });

            return Unit.Value;
        }
    }
}
