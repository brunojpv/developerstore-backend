using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Events;
using DeveloperStore.Application.UseCases.Sales;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Infrastructure.Data;
using MediatR;

namespace DeveloperStore.Infrastructure.Handlers
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleResponseDto>
    {
        private readonly AppDbContext _context;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public CreateSaleCommandHandler(AppDbContext context, IEventPublisher eventPublisher, IMapper mapper)
        {
            _context = context;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }

        public async Task<SaleResponseDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var sale = new Sale
            {
                SaleNumber = dto.SaleNumber,
                Date = dto.Date,
                CustomerId = dto.CustomerId,
                Branch = dto.Branch,
                Cancelled = false,
                Items = dto.Items.Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishAsync(new SaleCreatedEvent
            {
                SaleId = sale.Id,
                SaleNumber = sale.SaleNumber,
                Date = sale.Date
            });

            return _mapper.Map<SaleResponseDto>(sale);
        }
    }
}
