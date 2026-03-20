using ApiMovies.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ApiMovies.Common.Extensions;

// Ajustes de serialización JSON, caché de respuestas y respuestas 400 coherentes con ApiResponse.
public static class ControllerServiceCollectionExtensions
{
    // Registra controladores con perfiles de caché, enums como strings y manejo de ModelState inválido.
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                // Perfil reutilizable en [ResponseCache(CacheProfileName = "30Seconds")].
                options.CacheProfiles.Add("30Seconds", new CacheProfile { Duration = 30 });
            })
            .AddJsonOptions(options =>
            {
                // No envía propiedades null en JSON; enums legibles como texto.
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                // Sustituye el 400 por defecto de validación por el mismo envelope que el resto de la API.
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(entry => entry.Value?.Errors.Count > 0)
                        .ToDictionary(
                            entry => entry.Key,
                            entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
                        );

                    return new BadRequestObjectResult(
                        ApiResponse.Fail(
                            title: "One or more validation errors occurred.",
                            status: StatusCodes.Status400BadRequest,
                            data: errors
                        )
                    );
                };
            });

        return services;
    }
}
