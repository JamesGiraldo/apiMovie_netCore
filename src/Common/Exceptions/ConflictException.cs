namespace ApiMovies.Common.Exceptions;

// Conflicto con el estado actual del recurso (duplicados, reglas de unicidad); HTTP 409.
public class ConflictException : AppException
{
    // Parámetro detail: Motivo del conflicto (p. ej. email ya registrado).
    public ConflictException(string detail)
        : base(
            errorCode: "Conflict",
            errorTitle: "Resource conflict.",
            statusCode: StatusCodes.Status409Conflict,
            detail: detail
        ) {
    }
}
