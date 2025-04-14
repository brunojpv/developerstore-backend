using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductResponse> CreateAsync(ProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            var created = await _repository.AddAsync(product);
            return _mapper.Map<ProductResponse>(created);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return _mapper.Map<List<ProductResponse>>(products);
        }

        public async Task<ProductResponse?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return product is null ? null : _mapper.Map<ProductResponse>(product);
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _repository.GetCategoriesAsync();
        }

        public async Task<List<ProductResponse>> GetByCategoryAsync(string category)
        {
            var list = await _repository.GetByCategoryAsync(category);
            return _mapper.Map<List<ProductResponse>>(list);
        }

        public async Task UpdateAsync(int id, ProductRequest request)
        {
            var product = await _repository.GetByIdAsync(id) ?? throw new Exception("Product not found");
            _mapper.Map(request, product);
            await _repository.UpdateAsync(product);
        }
    }
}
