using ApiMovies.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ApiMovies.Common.Extensions;

public static class ControllerServiceCollectionExtensions
{
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.CacheProfiles.Add("30Seconds", new CacheProfile { Duration = 30 });
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .ConfigureApiBehaviorOptions(options =>
            {
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
