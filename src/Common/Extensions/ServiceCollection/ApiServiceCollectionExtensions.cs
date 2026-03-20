namespace ApiMovies.Common.Extensions;

// Punto único que compone la API: infraestructura (BD, storage, repos), seguridad (Identity, JWT) y capa web.
public static class ApiServiceCollectionExtensions
{
    // Registra todos los servicios necesarios para ejecutar la aplicación.
    public static IServiceCollection AddApiApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext, S3, repositorios y servicios de dominio.
        services.AddInfrastructure(configuration);
        // ASP.NET Identity + autenticación JWT.
        services.AddSecurity(configuration);
        // Controladores, Swagger, CORS, versionado, AutoMapper.
        services.AddPresentation();

        return services;
    }
}
