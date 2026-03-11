namespace ApiMovies.Common.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string detail)
        : base(
            errorCode: "NotFound",
            errorTitle: "Resource not found.",
            statusCode: StatusCodes.Status404NotFound,
            detail: detail
        ) {
    }
}
