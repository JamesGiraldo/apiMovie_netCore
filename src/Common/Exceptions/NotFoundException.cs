namespace ApiMovies.Common.Exceptions;

// Recurso solicitado inexistente o no accesible en el contexto actual; HTTP 404.
public class NotFoundException : AppException
{
    // Parámetro detail: Qué entidad o identificador no se encontró.
    public NotFoundException(string detail)
        : base(
            errorCode: "NotFound",
            errorTitle: "Resource not found.",
            statusCode: StatusCodes.Status404NotFound,
            detail: detail
        ) {
    }
}
