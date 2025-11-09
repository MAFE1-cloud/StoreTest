using AutoMapper;
using SalesHub.Application.DTOs;
using SalesHub.Domain.Entities;
using SalesHub.Domain.Interfaces;


namespace SalesHub.Application.Features.Products;

public class ProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto)
    {
        var entity = _mapper.Map<Product>(dto);
        await _repository.AddAsync(entity);
        return _mapper.Map<ProductDto>(entity);
    }

    public async Task UpdateAsync(ProductDto dto)
    {
        var existing = await _repository.GetByIdAsync(dto.Id);
        if (existing == null)
            throw new Exception("Producto no encontrado.");

        var entity = _mapper.Map<Product>(dto);

        // Actualiza sin duplicar tracking
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product != null)
            await _repository.DeleteAsync(product);
    }
}
