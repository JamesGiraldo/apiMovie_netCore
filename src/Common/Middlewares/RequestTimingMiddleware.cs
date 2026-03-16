using System.Diagnostics;

namespace ApiMovies.Common.Middlewares;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;
    private const int SlowRequestThresholdMs = 1000;
    private const string ResetColor = "\u001b[0m";
    private const string Cyan = "\u001b[36m";
    private const string Green = "\u001b[32m";
    private const string Yellow = "\u001b[33m";
    private const string Red = "\u001b[31m";

    public RequestTimingMiddleware(
        RequestDelegate next,
        ILogger<RequestTimingMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        Exception? requestException = null;
        var startedAtUtc = DateTime.UtcNow;
        var endpoint = $"{context.Request.Path}{context.Request.QueryString}";

        _logger.LogInformation(
            "{Message}",
            $"{Cyan}│ NEW HIT [{startedAtUtc:O}] @ {endpoint} - id({context.TraceIdentifier}){ResetColor}"
        );

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            requestException = ex;
            throw;
        }
        finally
        {
            sw.Stop();

            var elapsedMs = sw.Elapsed.TotalMilliseconds;
            var elapsedMsRounded = sw.ElapsedMilliseconds;
            var endedAtUtc = DateTime.UtcNow;
            var statusCode = requestException is null
                ? context.Response.StatusCode
                : StatusCodes.Status500InternalServerError;
            var logLevel = GetLogLevel(statusCode, elapsedMs);
            var completedColor = GetColor(logLevel);

            _logger.Log(
                logLevel,
                "{Message}",
                $"{completedColor}│ HIT COMPLETED [{endedAtUtc:O}] [{elapsedMsRounded}ms] @ {endpoint} - id({context.TraceIdentifier}){ResetColor}"
            );
        }
    }

    private static LogLevel GetLogLevel(int statusCode, double elapsedMs)
    {
        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            return LogLevel.Error;
        }

        if (statusCode >= StatusCodes.Status400BadRequest)
        {
            return LogLevel.Warning;
        }

        if (elapsedMs >= SlowRequestThresholdMs)
        {
            return LogLevel.Warning;
        }

        return LogLevel.Information;
    }

    private static string GetColor(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Error => Red,
            LogLevel.Warning => Yellow,
            _ => Green
        };
    }
}