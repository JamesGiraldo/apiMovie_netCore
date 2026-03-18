using ApiMovies.Common.Exceptions;
using ApiMovies.Common.Responses;
using System.Text.Json;

namespace ApiMovies.Common.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger
    ) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        } catch (AppException ex) {
            _logger.LogWarning(
                "Handled application exception {ErrorCode} [{StatusCode}] for {Method} {Path} | TraceId: {TraceId} | Detail: {Detail}",
                ex.ErrorCode,
                ex.StatusCode,
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier,
                ex.Message
            );
            await WriteResponseAsync(context, ex.StatusCode, ex.ErrorTitle, ex.Message);
        } catch (Exception ex) {
            _logger.LogError(
                ex,
                "Unhandled exception for {Method} {Path} | TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier
            );
            await WriteResponseAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Unexpected server error.",
                "An unexpected error occurred while processing the request."
            );
        }
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        int statusCode,
        string title,
        string detail
    ) {
        if (context.Response.HasStarted) {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = ApiResponse.Fail(
            title: title,
            status: statusCode,
            detail: detail
        );

        var json = JsonSerializer.Serialize(payload);
        await context.Response.WriteAsync(json);
    }
}
