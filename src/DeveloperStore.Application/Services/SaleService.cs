using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;

        public SaleService(ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SaleResponse> CreateAsync(SaleRequest request)
        {
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = $"S-{DateTime.UtcNow.Ticks}",
                Customer = request.Customer,
                Branch = request.Branch,
            };

            foreach (var item in request.Items)
            {
                if (item.Quantity > 20)
                    throw new InvalidOperationException("Maximum quantity per product is 20.");

                var saleItem = new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                saleItem.ApplyDiscount();
                sale.Items.Add(saleItem);
            }

            await _repository.AddAsync(sale);

            return _mapper.Map<SaleResponse>(sale);
        }

        public async Task<List<SaleResponse>> GetAllAsync()
        {
            var sales = await _repository.GetAllAsync();
            return _mapper.Map<List<SaleResponse>>(sales);
        }

        public async Task<SaleResponse?> GetByIdAsync(Guid id)
        {
            var sale = await _repository.GetByIdAsync(id);
            return sale is null ? null : _mapper.Map<SaleResponse>(sale);
        }

        public async Task UpdateAsync(Guid id, SaleRequest request)
        {
            var sale = await _repository.GetByIdAsync(id) ?? throw new Exception("Sale not found");
            sale.Customer = request.Customer;
            sale.Branch = request.Branch;

            sale.Items.Clear();
            foreach (var item in request.Items)
            {
                if (item.Quantity > 20)
                    throw new InvalidOperationException("Maximum quantity per product is 20.");

                var saleItem = new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                saleItem.ApplyDiscount();
                sale.Items.Add(saleItem);
            }

            await _repository.UpdateAsync(sale);
        }

        public async Task CancelAsync(Guid id)
        {
            await _repository.CancelAsync(id);
        }
    }
}
