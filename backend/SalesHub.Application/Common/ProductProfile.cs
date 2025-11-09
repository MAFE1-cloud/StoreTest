using AutoMapper;
using SalesHub.Application.DTOs;
using SalesHub.Domain.Entities;

namespace SalesHub.Application.Common.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
