using ApiMovies.Common.Options;
using ApiMovies.Interfaces.Services;
using ApiMovies.Services;

namespace ApiMovies.Common.Extensions;

// Integración con almacenamiento de archivos (S3) vía IFileStorageService.
public static class StorageServiceCollectionExtensions
{
    // Enlaza la sección "Storage" de configuración y registra el servicio de subida/descarga.
    public static IServiceCollection AddStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StorageOptions>(configuration.GetSection("Storage"));
        services.AddSingleton<IFileStorageService, S3FileStorageService>();

        return services;
    }
}
