using Microsoft.OpenApi;

namespace ApiMovies.Common.Extensions;

public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddApiSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                Type = SecuritySchemeType.Http
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer", document, null),
                    new List<string>()
                }
            });

            options.SwaggerDoc("v1", new OpenApiInfo {
                Version = "v1",
                Title = "Api Movies v1",
                Description = "Api for managing movies",
                Contact = new OpenApiContact {
                    Name = "James Giraldo",
                    Email = "jamesgiraldo@gmail.com",
                    Url = new Uri("https://github.com/jamesgiraldo")
                },
                License = new OpenApiLicense {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            options.SwaggerDoc("v2", new OpenApiInfo {
                Version = "v2",
                Title = "Api Movies v2",
                Description = "Api for managing movies",
                Contact = new OpenApiContact {
                    Name = "James Giraldo",
                    Email = "jamesgiraldo@gmail.com",
                    Url = new Uri("https://github.com/jamesgiraldo")
                },
                License = new OpenApiLicense {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });
        });

        return services;
    }

}
