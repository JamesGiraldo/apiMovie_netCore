using ApiMovies.Common.Extensions;

// Punto de entrada: crea el host web y registra servicios (DI), middleware y endpoints.
var builder = WebApplication.CreateBuilder(args);

// Serilog lee appsettings y enriquece logs (útil en producción y depuración).
builder.Host.UseApiSerilog();
// Registra EF Core, Identity, JWT, Swagger, CORS, controladores, repositorios y servicios de negocio.
builder.Services.AddApiApplication(builder.Configuration);

// Construye la aplicación con la configuración acumulada en builder.
var app = builder.Build();

// Encadena middleware (Swagger en dev, excepciones, HTTPS, CORS, auth, rutas de controladores).
app.UseApiApplication();
// Arranca el servidor Kestrel y empieza a escuchar peticiones HTTP.
app.Run();
