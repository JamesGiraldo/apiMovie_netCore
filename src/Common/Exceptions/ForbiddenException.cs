namespace ApiMovies.Common.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string detail = "You do not have permissions to access this resource.")
        : base(
            errorCode: "Forbidden",
            errorTitle: "Forbidden.",
            statusCode: StatusCodes.Status403Forbidden,
            detail: detail
        ) {
    }
}
