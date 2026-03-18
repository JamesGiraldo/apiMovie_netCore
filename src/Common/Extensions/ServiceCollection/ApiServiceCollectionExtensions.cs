namespace ApiMovies.Common.Extensions;

public static class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddApiApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddSecurity(configuration);
        services.AddPresentation();

        return services;
    }
}
