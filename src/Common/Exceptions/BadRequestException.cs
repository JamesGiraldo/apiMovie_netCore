namespace ApiMovies.Common.Exceptions;

// Solicitud inválida (validación de negocio o parámetros); se responde con HTTP 400.
public class BadRequestException : AppException
{
    // Parámetro detail: Descripción concreta de qué falló en la petición.
    public BadRequestException(string detail)
        : base(
            errorCode: "BadRequest",
            errorTitle: "Invalid request.",
            statusCode: StatusCodes.Status400BadRequest,
            detail: detail
        ) {
    }
}
