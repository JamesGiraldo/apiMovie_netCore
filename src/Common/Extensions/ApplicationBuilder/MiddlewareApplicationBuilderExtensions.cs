using ApiMovies.Common.Middlewares;

namespace ApiMovies.Common.Extensions;

// Registra middlewares personalizados en el orden correcto (antes de auth y controladores).
public static class MiddlewareApplicationBuilderExtensions
{
    // Encadena middlewares transversales: errores unificados y trazas de rendimiento por request.
    public static WebApplication UseApiMiddlewares(this WebApplication app)
    {
        // Convierte AppException y errores no controlados en JSON con el formato ApiResponse (común en toda la API).
        app.UseMiddleware<GlobalExceptionMiddleware>();
        // Registra inicio/fin de cada petición, duración y nivel de log según status / lentitud.
        app.UseMiddleware<RequestTimingMiddleware>();

        return app;
    }
}
