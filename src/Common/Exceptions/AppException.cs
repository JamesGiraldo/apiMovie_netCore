namespace ApiMovies.Common.Exceptions;

public abstract class AppException : Exception
{
    public int StatusCode { get; }
    public string ErrorCode { get; }
    public string ErrorTitle { get; }

    protected AppException(
        string errorCode,
        string errorTitle,
        int statusCode,
        string? detail = null,
        Exception? innerException = null
    ) : base(detail, innerException) {
        ErrorCode = errorCode;
        ErrorTitle = errorTitle;
        StatusCode = statusCode;
    }
}
