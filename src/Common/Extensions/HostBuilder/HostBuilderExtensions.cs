using Serilog;

namespace ApiMovies.Common.Extensions;

// Configura Serilog como proveedor de logging del host (reemplaza en parte el log por defecto).
public static class HostBuilderExtensions
{
    // Lee niveles, sinks y enrichers desde configuración y el contenedor de servicios.
    public static IHostBuilder UseApiSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
    }
}
