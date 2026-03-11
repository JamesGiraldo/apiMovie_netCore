namespace ApiMovies.Common.Responses;

public class ApiResponse
{
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public bool Success { get; set; }
    public string? Detail { get; set; }
    public object? Data { get; set; }

    public static ApiResponse Ok(
        string title,
        int status = StatusCodes.Status200OK,
        object? data = null,
        string? detail = null
    ) {
        return new ApiResponse {
            Title = title,
            Status = status,
            Success = true,
            Detail = detail,
            Data = data
        };
    }

    public static ApiResponse Fail(
        string title,
        int status,
        string? detail = null,
        object? data = null
    ) {
        return new ApiResponse {
            Title = title,
            Status = status,
            Success = false,
            Detail = detail,
            Data = data
        };
    }
}
