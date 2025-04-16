using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Interfaces;
using MediatR;

namespace DeveloperStore.Application.UseCases.Products
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public string? Filter { get; set; }
    }

    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepo.GetAllAsync(request.Filter);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}