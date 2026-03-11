namespace ApiMovies.Common.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string detail)
        : base(
            errorCode: "Conflict",
            errorTitle: "Resource conflict.",
            statusCode: StatusCodes.Status409Conflict,
            detail: detail
        ) {
    }
}
