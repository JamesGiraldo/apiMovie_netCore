namespace ApiMovies.Common.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string detail = "Authentication is required to access this resource.")
        : base(
            errorCode: "Unauthorized",
            errorTitle: "Unauthorized.",
            statusCode: StatusCodes.Status401Unauthorized,
            detail: detail
        ) {
    }
}
