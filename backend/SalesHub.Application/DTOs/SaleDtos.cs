namespace SalesHub.Application.DTOs;

public class CreateSaleItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateSaleDto
{
    public DateTime? Date { get; set; } // opcional (default: UtcNow)
    public List<CreateSaleItemDto> Items { get; set; } = new();
}

public class SaleItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}

public class SaleDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public List<SaleItemDto> Items { get; set; } = new();
}

public class SalesReportRowDto
{
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
}

public class SalesReportDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal GrandTotal { get; set; }
    public List<SalesReportRowDto> Rows { get; set; } = new();
}
