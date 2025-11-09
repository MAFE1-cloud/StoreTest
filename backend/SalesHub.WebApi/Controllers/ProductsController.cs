using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesHub.Application.DTOs;
using SalesHub.Application.Features.Products;
using SalesHub.Infrastructure.Services;

namespace SalesHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;
    private readonly AzureBlobService _blobService;

    public ProductsController(ProductService service, AzureBlobService blobService)
    {
        _service = service;
        _blobService = blobService;
    }

    // ✅ Consultar todos los productos
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    // ✅ Consultar un producto por ID
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var product = await _service.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    // ✅ Crear producto (solo admin)
    [Authorize(Roles = "admin")]
    [HttpPost]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> Post([FromForm] ProductDto dto, IFormFile? image)
    {
        if (image != null)
        {
            // Sube imagen a carpeta "mafe/"
            var imageUrl = await _blobService.UploadAsync(image);
            dto.ImageUrl = imageUrl;
        }

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    // ✅ Actualizar producto (solo admin)
    [Authorize(Roles = "admin")]
    [HttpPut("{id:guid}")]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> Put(Guid id, [FromForm] ProductDto dto, IFormFile? image)
    {
        if (id != dto.Id)
            return BadRequest("El ID del producto no coincide.");

        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        if (image != null)
        {
            // Borrar la imagen anterior (si existe)
            await _blobService.DeleteAsync(existing.ImageUrl);

            // Subir nueva imagen al mismo folder "mafe/"
            var newImageUrl = await _blobService.UploadAsync(image);
            dto.ImageUrl = newImageUrl;
        }
        else
        {
            dto.ImageUrl = existing.ImageUrl; // conservar la actual
        }

        await _service.UpdateAsync(dto);
        return NoContent();
    }

    // ✅ Eliminar producto (solo admin)
    [Authorize(Roles = "admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        // Eliminar imagen en Azure Blob
        await _blobService.DeleteAsync(product.ImageUrl);

        await _service.DeleteAsync(id);
        return NoContent();
    }
}
