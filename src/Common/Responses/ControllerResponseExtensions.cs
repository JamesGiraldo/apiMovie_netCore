using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiMovies.Common.Responses;

public static class ControllerResponseExtensions
{
    public static IActionResult ApiSuccess(
        this ControllerBase controller,
        string title,
        object? data = null,
        string? detail = null,
        int statusCode = StatusCodes.Status200OK
    ) {
        var response = ApiResponse.Ok(title, statusCode, data, detail);
        return new ObjectResult(response) { StatusCode = statusCode };
    }

    public static IActionResult ApiFailure(
        this ControllerBase controller,
        string title,
        int statusCode,
        string? detail = null,
        object? data = null
    ) {
        var response = ApiResponse.Fail(title, statusCode, detail, data);
        return new ObjectResult(response) { StatusCode = statusCode };
    }

    public static IActionResult ApiValidationFailure(
        this ControllerBase controller,
        ModelStateDictionary modelState,
        int statusCode = StatusCodes.Status400BadRequest,
        string title = "One or more validation errors occurred.",
        string? detail = null
    ) {
        var errors = modelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .ToDictionary(
                entry => entry.Key,
                entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
            );

        var response = ApiResponse.Fail(title, statusCode, detail, errors);
        return new ObjectResult(response) { StatusCode = statusCode };
    }
}
