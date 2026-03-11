using ApiMovies.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Common.Responses;

public static class ServiceResultControllerExtensions
{
    public static IActionResult FromServiceResult<T>(
        this ControllerBase controller,
        ServiceResult<T> result,
        string successTitle,
        int successStatus = StatusCodes.Status200OK
    ) {
        if (result.Succeeded)
        {
            return controller.ApiSuccess(
                title: successTitle,
                statusCode: successStatus,
                data: result.Value,
                detail: result.Detail
            );
        }

        return controller.ApiFailure(
            title: result.Title ?? "Request failed.",
            statusCode: MapStatusCode(result.ErrorCode),
            detail: result.Detail
        );
    }

    private static int MapStatusCode(string? errorCode) => errorCode switch {
        "InvalidPayload" or "InvalidName" or "InvalidId" or "RouteBodyIdMismatch" => StatusCodes.Status400BadRequest,
        "NotFound" => StatusCodes.Status404NotFound,
        "DuplicateName" => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };
}
