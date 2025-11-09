using SalesHub.Application.DTOs;
using SalesHub.Domain.Entities;
using SalesHub.Domain.Interfaces;

namespace SalesHub.Application.Features.Sales;

public class SaleService
{
    private readonly IProductRepository _productRepo;
    private readonly IRepository<Sale> _saleRepo;
    private readonly IRepository<SaleItem> _itemRepo;

    public SaleService(
        IProductRepository productRepo,
        IRepository<Sale> saleRepo,
        IRepository<SaleItem> itemRepo)
    {
        _productRepo = productRepo;
        _saleRepo = saleRepo;
        _itemRepo = itemRepo;
    }

    // ✅ Crear una venta
    public async Task<SaleDto> CreateAsync(CreateSaleDto dto)
    {
        var products = (await _productRepo.GetAllAsync()).ToDictionary(p => p.Id);

        var sale = new Sale
        {
            Date = dto.Date ?? DateTime.UtcNow,
            Items = new List<SaleItem>()
        };

        decimal total = 0;

        foreach (var item in dto.Items)
        {
            if (!products.ContainsKey(item.ProductId))
                throw new InvalidOperationException("Producto no encontrado");

            var product = products[item.ProductId];

            if (product.Stock < item.Quantity)
                throw new InvalidOperationException($"Stock insuficiente para {product.Name}");

            product.Stock -= item.Quantity;
            var subtotal = product.Price * item.Quantity;

            sale.Items.Add(new SaleItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                Subtotal = subtotal
            });

            total += subtotal;
        }

        sale.Total = total;
        await _saleRepo.AddAsync(sale);

        // 🔹 Devuelve la venta lista para el PDF
        var saleDto = new SaleDto
        {
            Id = sale.Id,
            Date = sale.Date,
            Total = sale.Total,
            Items = sale.Items.Select(i => new SaleItemDto
            {
                ProductId = i.ProductId,
                ProductName = products[i.ProductId].Name,
                Quantity = i.Quantity,
                Subtotal = i.Subtotal
            }).ToList()
        };

        return saleDto;
    }

    // ✅ Obtener una venta por ID
    public async Task<SaleDto?> GetByIdAsync(Guid id)
    {
        var sales = await _saleRepo.GetAllAsync();
        var sale = sales.FirstOrDefault(s => s.Id == id);
        if (sale == null) return null;

        return new SaleDto
        {
            Id = sale.Id,
            Date = sale.Date,
            Total = sale.Total,
            Items = sale.Items.Select(i => new SaleItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Subtotal = i.Subtotal
            }).ToList()
        };
    }

    // ✅ Reporte de ventas
    public async Task<SalesReportDto> ReportAsync(DateTime from, DateTime to)
    {
        var sales = await _saleRepo.GetAllAsync();
        var filtered = sales
            .Where(s => s.Date >= from && s.Date <= to)
            .GroupBy(s => s.Date.Date)
            .Select(g => new SalesReportRowDto
            {
                Date = g.Key,
                Total = g.Sum(x => x.Total)
            })
            .OrderBy(r => r.Date)
            .ToList();

        return new SalesReportDto
        {
            From = from,
            To = to,
            GrandTotal = filtered.Sum(r => r.Total),
            Rows = filtered
        };
    }
}
