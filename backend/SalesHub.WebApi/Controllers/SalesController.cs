using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesHub.Application.DTOs;
using SalesHub.Application.Features.Sales;
using SalesHub.Infrastructure.Services; // 👈 Importante para el PdfGeneratorService
using System.Linq;
using static SalesHub.Infrastructure.Services.PdfGeneratorService;

namespace SalesHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly SaleService _saleService;

    public SalesController(SaleService saleService)
    {
        _saleService = saleService;
    }

    /// <summary>
    /// Crear una nueva venta (solo ADMIN)
    /// </summary>
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleDto dto)
    {
        var result = await _saleService.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>
    /// Obtener todas las ventas registradas
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Por simplicidad usamos el reporte completo
        var sales = await _saleService.ReportAsync(DateTime.MinValue, DateTime.MaxValue);
        return Ok(sales);
    }

    /// <summary>
    /// Obtener una venta específica por ID
    /// </summary>
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var sale = await _saleService.GetByIdAsync(id);
        return sale == null ? NotFound() : Ok(sale);
    }

    /// <summary>
    /// Generar comprobante PDF de una venta
    /// </summary>
    [Authorize]
    [HttpGet("{id:guid}/receipt")]
    public async Task<IActionResult> GetReceipt(Guid id)
    {
        var sale = await _saleService.GetByIdAsync(id);
        if (sale == null)
            return NotFound("Venta no encontrada.");

        // 🔹 Adaptamos a DTO simple para evitar ciclo con Application
        var simpleSale = new SimpleSaleDto
        {
            Id = sale.Id,
            Date = sale.Date,
            Total = sale.Total,
            Items = sale.Items.Select(i => new SimpleSaleItemDto
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Subtotal = i.Subtotal
            }).ToList()
        };

        var pdfBytes = PdfGeneratorService.GenerateSaleReceipt(simpleSale);
        return File(pdfBytes, "application/pdf", $"Comprobante_{sale.Id}.pdf");
    }

    /// <summary>
    /// Reporte de ventas por rango de fechas
    /// </summary>
    [Authorize(Roles = "admin")]
    [HttpGet("report")]
    public async Task<IActionResult> Report([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        if (from == default || to == default)
            return BadRequest("Debe especificar un rango de fechas válido.");

        var report = await _saleService.ReportAsync(from, to);
        return Ok(report);
    }
}
