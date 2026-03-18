using ApiMovies.MoviesMappers;

namespace ApiMovies.Common.Extensions;

public static class PresentationServiceCollectionExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(MoviesMapper).Assembly);
        services.AddApiControllers();
        services.AddEndpointsApiExplorer();
        services.AddApiSwagger();
        services.AddApiCors();
        services.AddApiVersioningConfig();

        return services;
    }
}
