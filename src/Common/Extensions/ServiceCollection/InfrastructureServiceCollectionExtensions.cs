using ApiMovies.Data;

namespace ApiMovies.Common.Extensions;

// Capa de infraestructura: persistencia, almacenamiento de archivos y acceso a datos vía repositorios.
public static class InfrastructureServiceCollectionExtensions
{
    // Registra base de datos PostgreSQL, opciones de storage y las implementaciones de repos/servicios.
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgresDatabase(configuration);
        services.AddStorageServices(configuration);
        services.AddRepositories();
        services.AddServices();

        return services;
    }
}
