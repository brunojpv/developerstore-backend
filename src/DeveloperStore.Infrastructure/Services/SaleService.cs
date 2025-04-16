using DeveloperStore.Application.Events;
using DeveloperStore.Application.Interfaces;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Infrastructure.Services
{
    public class SaleService : ISaleService
    {
        private readonly AppDbContext _context;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<SaleService> _logger;

        public SaleService(AppDbContext context, IEventPublisher eventPublisher, ILogger<SaleService> logger)
        {
            _context = context;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Venda criada: {SaleNumber}", sale.SaleNumber);

            await _eventPublisher.PublishAsync(new SaleCreatedEvent
            {
                SaleId = sale.Id,
                SaleNumber = sale.SaleNumber,
                Date = sale.Date
            });

            return sale;
        }

        public async Task CancelSaleAsync(int saleId, string reason)
        {
            var sale = _context.Sales.FirstOrDefault(s => s.Id == saleId);
            if (sale is null)
            {
                throw new InvalidOperationException("Venda não encontrada.");
            }

            sale.Cancelled = true;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Venda cancelada: {SaleId}", sale.Id);

            await _eventPublisher.PublishAsync(new SaleCancelledEvent
            {
                SaleId = sale.Id,
                Reason = reason
            });
        }

        public async Task ModifySaleAsync(Sale sale, string modifiedBy)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Venda modificada: {SaleId}", sale.Id);

            await _eventPublisher.PublishAsync(new SaleModifiedEvent
            {
                SaleId = sale.Id,
                ModifiedBy = modifiedBy,
                ModifiedAt = DateTime.UtcNow
            });
        }
    }
}
