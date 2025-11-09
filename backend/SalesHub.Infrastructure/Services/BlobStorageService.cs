using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace SalesHub.Infrastructure.Services;

public class AzureBlobService
{
    private readonly string _connectionString;
    private readonly string _containerName = "testindigo";

    public AzureBlobService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        // ❌ Quitar la creación (tu SAS no tiene permisos para crear el contenedor)
        // await containerClient.CreateIfNotExistsAsync();

        // 📁 Guardar dentro de la carpeta mafe/
        var blobName = $"mafe/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var blobClient = containerClient.GetBlobClient(blobName);

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString(); // ✅ URL directa (sin SAS duplicado)
    }

    public async Task DeleteAsync(string? imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return;

        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        var blobName = imageUrl.Split($"{_containerName}/")[1];
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }
}
