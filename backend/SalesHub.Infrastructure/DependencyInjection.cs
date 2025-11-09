using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesHub.Domain.Interfaces;
using SalesHub.Infrastructure.Persistence;
using SalesHub.Infrastructure.Repositories;
using SalesHub.Infrastructure.Services;

namespace SalesHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 💾 Configurar DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // 🧱 Repositorios genéricos
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        // 📦 Repositorio de productos
        services.AddScoped<IProductRepository, ProductRepository>();

        // 🪣 Servicio de blob storage (solo pruebas)
        services.AddSingleton(new AzureBlobService(
            "https://pruebaindigost.blob.core.windows.net/testindigo" +
            "?sp=racwdli&st=2025-11-08T02:41:42Z&se=2025-11-09T01:00:00Z" +
            "&spr=https&sv=2024-11-04&sr=c&sig=eUn3Pd4N0MfYfNLg7XNobSph9HGudetiFAKQOF0uDZY%3D"
        ));

        return services;
    }
}
