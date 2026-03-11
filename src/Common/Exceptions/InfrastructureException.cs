namespace ApiMovies.Common.Exceptions;

public class InfrastructureException : AppException
{
    public InfrastructureException(
        string detail,
        Exception? innerException = null
    ) : base(
        errorCode: "InfrastructureError",
        errorTitle: "Infrastructure error.",
        statusCode: StatusCodes.Status500InternalServerError,
        detail: detail,
        innerException: innerException
    ) {
    }
}
