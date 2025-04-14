using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CartResponse> CreateAsync(CartRequest request)
        {
            var cart = _mapper.Map<Cart>(request);
            var created = await _repository.AddAsync(cart);
            return _mapper.Map<CartResponse>(created);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<CartResponse>> GetAllAsync()
        {
            var carts = await _repository.GetAllAsync();
            return _mapper.Map<List<CartResponse>>(carts);
        }

        public async Task<CartResponse?> GetByIdAsync(int id)
        {
            var cart = await _repository.GetByIdAsync(id);
            return cart is null ? null : _mapper.Map<CartResponse>(cart);
        }

        public async Task UpdateAsync(int id, CartRequest request)
        {
            var cart = await _repository.GetByIdAsync(id) ?? throw new Exception("Cart not found");
            _mapper.Map(request, cart);
            await _repository.UpdateAsync(cart);
        }
    }
}
