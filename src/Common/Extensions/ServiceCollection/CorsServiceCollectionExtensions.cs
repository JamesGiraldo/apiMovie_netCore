namespace ApiMovies.Common.Extensions;

public static class CorsServiceCollectionExtensions
{
    public static IServiceCollection AddApiCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            });
        });

        return services;
    }
}
