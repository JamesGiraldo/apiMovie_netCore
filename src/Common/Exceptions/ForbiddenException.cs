namespace ApiMovies.Common.Exceptions;

// El usuario autenticado no tiene permiso para la operación; HTTP 403.
public class ForbiddenException : AppException
{
    // Parámetro detail: Mensaje opcional; por defecto indica falta de permisos.
    public ForbiddenException(string detail = "You do not have permissions to access this resource.")
        : base(
            errorCode: "Forbidden",
            errorTitle: "Forbidden.",
            statusCode: StatusCodes.Status403Forbidden,
            detail: detail
        ) {
    }
}
