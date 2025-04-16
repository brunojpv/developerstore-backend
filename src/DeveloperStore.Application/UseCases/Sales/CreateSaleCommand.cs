using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using MediatR;

namespace DeveloperStore.Application.UseCases.Sales
{
    public class CreateSaleCommand : IRequest<SaleResponseDto>
    {
        public CreateSaleDto Dto { get; set; } = new();
    }

    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, SaleResponseDto>
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public CreateSaleHandler(ISaleRepository saleRepo, IProductRepository productRepo, IMapper mapper)
        {
            _saleRepo = saleRepo;
            _productRepo = productRepo;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SaleResponseDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var items = new List<SaleItem>();

            foreach (var item in dto.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product is null)
                    throw new ArgumentException($"Product {item.ProductId} not found.");

                items.Add(new SaleItem
                {
                    ProductId = item.ProductId,
                    ProductName = product.Title,
                    UnitPrice = product.Price,
                    Quantity = item.Quantity
                });
            }

            var sale = new Sale
            {
                SaleNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                CustomerId = dto.CustomerId,
                Branch = dto.Branch,
                Date = DateTime.UtcNow,
                Items = items
            };

            await _saleRepo.AddAsync(sale);
            return _mapper.Map<SaleResponseDto>(sale);
        }
    }
}
