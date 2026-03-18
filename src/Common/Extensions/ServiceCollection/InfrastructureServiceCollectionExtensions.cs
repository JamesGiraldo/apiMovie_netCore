using ApiMovies.Data;

namespace ApiMovies.Common.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgresDatabase(configuration);
        services.AddStorageServices(configuration);
        services.AddRepositories();
        services.AddServices();

        return services;
    }
}
