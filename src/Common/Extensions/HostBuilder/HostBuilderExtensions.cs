using Serilog;

namespace ApiMovies.Common.Extensions;

public static class HostBuilderExtensions
{
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
