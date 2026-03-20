namespace ApiMovies.Common.Extensions;

// Política CORS permisiva para desarrollo / demos (en producción conviene restringir orígenes).
public static class CorsServiceCollectionExtensions
{
    // Define la política AllowAll usada en app.UseCors("AllowAll").
    public static IServiceCollection AddApiCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                // Cualquier origen, método y cabecera (útil para SPA en otro puerto).
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            });
        });

        return services;
    }
}
