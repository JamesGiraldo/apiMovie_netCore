namespace ApiMovies.Services.IService;

public class ServiceResult<T>
{
    public bool Succeeded { get; private set; }
    public T? Value { get; private set; }
    public string? ErrorCode { get; private set; }
    public string? Title { get; private set; }
    public string? Detail { get; private set; }

    public static ServiceResult<T> Success(T value) => new() {
        Succeeded = true,
        Value = value
    };

    public static ServiceResult<T> Failure(string errorCode, string title, string? detail = null) => new() {
        Succeeded = false,
        ErrorCode = errorCode,
        Title = title,
        Detail = detail
    };
}
