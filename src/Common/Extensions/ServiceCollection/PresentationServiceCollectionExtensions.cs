using ApiMovies.MoviesMappers;

namespace ApiMovies.Common.Extensions;

// Todo lo relacionado con exponer la API: MVC, documentación, CORS y versionado.
public static class PresentationServiceCollectionExtensions
{
    // AutoMapper, controladores JSON, Swagger, CORS y API versioning.
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
