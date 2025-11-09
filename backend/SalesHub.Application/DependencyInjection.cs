using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SalesHub.Application.Features.Sales;
using SalesHub.Application.Features.Products;
using SalesHub.Application.Features.Auth;


namespace SalesHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<ProductService>(); 
        services.AddScoped<SaleService>();
        services.AddScoped<AuthService>();

        return services;
    }
}
