using System.IO;
using System.Reflection.Metadata;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Npgsql.Internal;

namespace SalesHub.Infrastructure.Services;

// 🔹 DTOs internos mínimos (sin dependencias circulares)
public class SimpleSaleDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public List<SimpleSaleItemDto> Items { get; set; } = new();
}

public class SimpleSaleItemDto
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}

public static class PdfGeneratorService
{
    public static byte[] GenerateSaleReceipt(SimpleSaleDto sale)
    {
        using var ms = new MemoryStream();
        var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 40, 40, 40, 40);
        iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);
        document.Open();


        // 🔹 Título
        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
        var title = new Paragraph("🧾 COMPROBANTE DE VENTA", titleFont)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 20
        };
        document.Add(title);

        // 🔹 Detalles
        document.Add(new Paragraph($"📅 Fecha: {sale.Date:dd/MM/yyyy HH:mm}"));
        document.Add(new Paragraph($"🆔 ID Venta: {sale.Id}"));
        document.Add(new Paragraph(" "));

        // 🔹 Tabla
        var table = new PdfPTable(3) { WidthPercentage = 100 };
        table.AddCell("Producto");
        table.AddCell("Cantidad");
        table.AddCell("Subtotal");

        foreach (var item in sale.Items)
        {
            table.AddCell(item.ProductName);
            table.AddCell(item.Quantity.ToString());
            table.AddCell($"${item.Subtotal:F2}");
        }

        document.Add(table);

        // 🔹 Total
        var totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
        var total = new Paragraph($"\n💰 TOTAL: ${sale.Total:F2}", totalFont)
        {
            Alignment = Element.ALIGN_RIGHT
        };
        document.Add(total);

        document.Close();
        return ms.ToArray();
    }
}
