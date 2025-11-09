# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Instalar la herramienta dotnet-ef
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Directorio de trabajo
WORKDIR /src

# Copiamos los archivos del proyecto
COPY StoreByMafe.sln ./
COPY SalesHub.WebApi/SalesHub.WebApi.csproj SalesHub.WebApi/
COPY SalesHub.Application/SalesHub.Application.csproj SalesHub.Application/
COPY SalesHub.Domain/SalesHub.Domain.csproj SalesHub.Domain/
COPY SalesHub.Infrastructure/SalesHub.Infrastructure.csproj SalesHub.Infrastructure/

# Restaurar dependencias
RUN dotnet restore "./StoreByMafe.sln"

# Copiar el resto del código
COPY . .

# Compilar en modo Release y aplicar migraciones
WORKDIR /src/SalesHub.WebApi

# Aplicar migraciones a la base de datos (usando el SDK)
RUN dotnet ef database update --project ../SalesHub.Infrastructure --startup-project . || true

RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Puerto de la aplicación
ENV ASPNETCORE_URLS=http://+:5073
EXPOSE 5073

ENTRYPOINT ["dotnet", "SalesHub.WebApi.dll"]
