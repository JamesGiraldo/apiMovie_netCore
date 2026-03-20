namespace ApiMovies.Common.Extensions;
// Ordena el pipeline HTTP de la API: documentación, manejo de errores, seguridad y enrutado a controladores.

public static class ApiApplicationBuilderExtensions
{
    // Aplica toda la configuración de middleware necesaria después de WebApplicationBuilder.Build.
    public static WebApplication UseApiApplication(this WebApplication app)
    {
        // Interfaz Swagger / OpenAPI solo donde corresponda (p. ej. Development).
        app.UseApiSwaggerUI();
        // Captura excepciones globales y mide tiempos de respuesta en logs.
        app.UseApiMiddlewares();
        // Redirige HTTP → HTTPS cuando el cliente use HTTP en un puerto seguro.
        app.UseHttpsRedirection();
        // Habilita CORS con la política "AllowAll" registrada en servicios (orígenes/métodos/headers).
        app.UseCors("AllowAll");
        // Valida el token JWT (si viene en la petición) y rellena User en HttpContext.
        app.UseAuthentication();
        // Evalúa [Authorize], roles y políticas después de saber quién es el usuario.
        app.UseAuthorization();
        // Registra las rutas de todos los controladores API (atributos [Route], [HttpGet], etc.).
        app.MapControllers();

        return app;
    }

}
