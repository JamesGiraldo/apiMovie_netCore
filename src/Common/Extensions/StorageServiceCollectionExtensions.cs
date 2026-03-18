using ApiMovies.Common.Options;
using ApiMovies.Interfaces.Services;
using ApiMovies.Services;

namespace ApiMovies.Common.Extensions;

public static class StorageServiceCollectionExtensions
{
    public static IServiceCollection AddStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StorageOptions>(configuration.GetSection("Storage"));
        services.AddSingleton<IFileStorageService, S3FileStorageService>();

        return services;
    }
}
