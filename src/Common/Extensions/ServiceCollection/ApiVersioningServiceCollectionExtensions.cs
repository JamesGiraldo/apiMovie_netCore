using Asp.Versioning;

namespace ApiMovies.Common.Extensions;

// Versionado de API (v1, v2 en URL o cabecera) y metadatos para Swagger.
public static class ApiVersioningServiceCollectionExtensions
{
    // Versión por defecto 1.0 si el cliente no la indica; explorer sustituye {version:apiVersion} en rutas.
    public static IServiceCollection AddApiVersioningConfig(this IServiceCollection services)
    {
        var apiVersioningBuilder = services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        apiVersioningBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
