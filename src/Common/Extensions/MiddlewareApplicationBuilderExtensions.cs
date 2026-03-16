using ApiMovies.Common.Middlewares;

namespace ApiMovies.Common.Extensions;

public static class MiddlewareApplicationBuilderExtensions
{
    public static WebApplication UseApiMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<RequestTimingMiddleware>();

        return app;
    }
}
