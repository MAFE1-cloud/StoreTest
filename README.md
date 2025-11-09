💜 StoreByMafe

StoreByMafe es una aplicación web completa de gestión de ventas y productos.
Desarrollada con una arquitectura moderna (Frontend en React + Vite y Backend en .NET 8 + PostgreSQL),
su objetivo es ofrecer una plataforma rápida, modular y segura para administración de tiendas.

📁 Estructura del proyecto
StoreByMafe/
│
├── backend/                  # API REST construida en .NET 8
│   ├── SalesHub.WebApi/
│   ├── SalesHub.Application/
│   ├── SalesHub.Infrastructure/
│   ├── SalesHub.Domain/
│   ├── StoreByMafe.sln
│   └── appsettings.json
│
├── frontend/                 # Interfaz de usuario en React + Vite + Tailwind
│   ├── src/
│   ├── public/
│   ├── package.json
│   └── vite.config.js
│
└── README.md


🧠 Arquitectura general
Capa	Descripción
Frontend (React + Vite)	Interfaz visual, comunicación con el backend mediante API REST, manejo de sesión JWT.
Backend (.NET 8 + EF Core)	Expone endpoints RESTful, gestiona autenticación, lógica de negocio y persistencia.
Base de datos (PostgreSQL)	Almacena usuarios, productos, ventas y registros de autenticación.

🚀 Tecnologías utilizadas
🖥️ Frontend
React 18
Vite
TailwindCSS
React Router DOM
Fetch API para comunicación con el backend.
---------------------------------

⚙️ Backend
.NET 8 Web API
Entity Framework Core
PostgreSQL 16
JWT Authentication
Arquitectura por capas (Domain, Application, Infrastructure, WebApi)

⚙️ Requisitos previos
Antes de comenzar, asegúrate de tener instalado:
🧰 .NET SDK 8.0+
🧩 Node.js 18+
🐘 PostgreSQL 16+
📦 npm o yarn para gestionar dependencias frontend

-------------------------------
🔧 Configuración del entorno local
1️⃣ Clonar el repositorio
git clone https://github.com/<TU_USUARIO>/<TU_REPOSITORIO>.git
cd StoreByMafe

2️⃣ Configurar la base de datos

Crea una base de datos local:
CREATE DATABASE storebym_db;


Abre backend/SalesHub.WebApi/appsettings.json y configura la conexión:
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=storebym_db;Username=postgres;Password=12345678"
},
"Jwt": {
  "Key": "supersecretkey12345678",
  "Issuer": "SalesHubAPI",
  "Audience": "SalesHubClient"
}


Aplica las migraciones con Entity Framework:
cd backend
dotnet ef database update --project SalesHub.Infrastructure --startup-project SalesHub.WebApi

3️⃣ Iniciar el backend
Desde la carpeta backend:
dotnet run --project SalesHub.WebApi

La API estará disponible en:
👉 http://localhost:5073/swagger

4️⃣ Configurar el frontend
Desde la carpeta frontend:
npm install


Crea un archivo .env con la URL del backend:
VITE_API_URL=http://localhost:5073/api


Luego inicia el servidor de desarrollo:
npm run dev


Accede en tu navegador a 👉 http://localhost:5173

🔐 Autenticación y uso
Registra un usuario administrador:
POST http://localhost:5073/api/Auth/register
{
  "email": "admin@test.com",
  "password": "12345678",
  "role": "admin"
}


Inicia sesión para obtener tu token JWT:
POST http://localhost:5073/api/Auth/login
{
  "email": "admin@test.com",
  "password": "12345678"
}


Usa el token en tus peticiones (en el header Authorization: Bearer <tu_token>)

📡 Endpoints principales
Método	Endpoint	Descripción
POST	/api/Auth/register	Registrar usuario
POST	/api/Auth/login	Iniciar sesión
GET	/api/Products	Listar productos
POST	/api/Products	Crear producto
GET	/api/Sales	Listar ventas
POST	/api/Sales	Crear venta
🎨 Interfaz visual

El frontend presenta una interfaz moderna y limpia creada con TailwindCSS,
usando una paleta de tonos lila y suaves 💜,
bajo la marca StoreByMafe, inspirada en simplicidad y usabilidad.

🧩 Estructura técnica del backend
SalesHub.Domain/           → Entidades base
SalesHub.Application/      → Casos de uso y lógica de negocio
SalesHub.Infrastructure/   → EF Core, Repositorios, Servicios externos
SalesHub.WebApi/           → Controladores, Configuración JWT, Swagger

🧩 Estructura técnica del frontend
src/
 ├── api/           → Conexión al backend (fetch)
 ├── components/    → UI y layout
 ├── pages/         → Vistas principales (Login, Productos, Ventas)
 ├── context/       → Manejo global de sesión (JWT)
 └── main.jsx       → Punto de entrada

🧰 Comandos útiles
🔄 Migraciones EF Core
dotnet ef migrations add NombreMigracion --project SalesHub.Infrastructure --startup-project SalesHub.WebApi
dotnet ef database update --project SalesHub.Infrastructure --startup-project SalesHub.WebApi

🧼 Limpieza del backend
dotnet clean
dotnet build

🧩 Frontend
npm run dev       # desarrollo
npm run build     # producción
npm run preview   # vista previa del build

☁️ Despliegue futuro (opcional)

Backend → Azure App Service / Render / Railway

Base de datos → Azure PostgreSQL / Supabase

Frontend → Vercel / Netlify

💜 Autora

Mafe
Desarrolladora full-stack, creadora de StoreByMafe ✨
Apasionada por las interfaces limpias, la arquitectura clara y el código mantenible.




