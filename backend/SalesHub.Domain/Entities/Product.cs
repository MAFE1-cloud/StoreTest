using SalesHub.Domain.Common;

namespace SalesHub.Domain.Entities;


public class Product : IEntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }

    public ICollection<SaleItem>? SaleItems { get; set; }
}
