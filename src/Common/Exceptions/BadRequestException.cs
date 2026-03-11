namespace ApiMovies.Common.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException(string detail)
        : base(
            errorCode: "BadRequest",
            errorTitle: "Invalid request.",
            statusCode: StatusCodes.Status400BadRequest,
            detail: detail
        ) {
    }
}
