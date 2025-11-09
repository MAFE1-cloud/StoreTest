using SalesHub.Domain.Entities;
using SalesHub.Domain.Interfaces;
using SalesHub.Infrastructure.Persistence;

namespace SalesHub.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
}
