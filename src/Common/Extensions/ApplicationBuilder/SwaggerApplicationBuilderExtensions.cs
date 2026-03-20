namespace ApiMovies.Common.Extensions;

// Expone Swagger UI en entornos de desarrollo para probar la API sin Postman.
public static class SwaggerApplicationBuilderExtensions
{
    // Activa el middleware de Swagger y la UI que apunta a los documentos v1 y v2.
    public static WebApplication UseApiSwaggerUI(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // Sirve el JSON OpenAPI generado (una especificación por versión de API).
            app.UseSwagger();
            // Página HTML interactiva; cada endpoint apunta a su swagger.json.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Movies v1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Api Movies v2");
            });
        }

        return app;
    }
}
