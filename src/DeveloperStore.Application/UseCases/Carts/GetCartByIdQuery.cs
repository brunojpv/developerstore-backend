using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Interfaces;
using MediatR;

namespace DeveloperStore.Application.UseCases.Carts
{
    public class GetCartByIdQuery : IRequest<CartDto>
    {
        public int Id { get; set; }
    }

    public class GetCartByIdHandler : IRequestHandler<GetCartByIdQuery, CartDto>
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;

        public GetCartByIdHandler(ICartRepository cartRepo, IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepo.GetByIdAsync(request.Id);
            if (cart == null) throw new Exception($"Cart {request.Id} not found");
            return _mapper.Map<CartDto>(cart);
        }
    }
}
