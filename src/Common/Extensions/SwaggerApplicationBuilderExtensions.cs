namespace ApiMovies.Common.Extensions;

public static class SwaggerApplicationBuilderExtensions
{
    public static WebApplication UseApiSwaggerUI(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Movies v1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Api Movies v2");
            });
        }

        return app;
    }
}
