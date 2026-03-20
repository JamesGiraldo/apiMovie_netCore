namespace ApiMovies.Common.Exceptions;

// Falta autenticación o credenciales inválidas; HTTP 401.
public class UnauthorizedException : AppException
{
    // Parámetro detail: Mensaje opcional (por defecto indica que se requiere autenticación).
    public UnauthorizedException(string detail = "Authentication is required to access this resource.")
        : base(
            errorCode: "Unauthorized",
            errorTitle: "Unauthorized.",
            statusCode: StatusCodes.Status401Unauthorized,
            detail: detail
        ) {
    }
}
