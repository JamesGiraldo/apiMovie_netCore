namespace ApiMovies.Common.Extensions;

public static class ApiApplicationBuilderExtensions
{
    public static WebApplication UseApiApplication(this WebApplication app)
    {
        app.UseApiSwaggerUI();
        app.UseApiMiddlewares();
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
