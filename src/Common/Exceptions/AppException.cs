namespace ApiMovies.Common.Exceptions;

// Excepción de dominio de la API: transporta código HTTP, código de error estable y título para respuestas uniformes.
// El middleware global traduce estas excepciones al formato ApiResponse.
public abstract class AppException : Exception
{
    // Código HTTP asociado (por ejemplo 404, 409).
    public int StatusCode { get; }
    // Identificador corto del error (p. ej. NotFound) para clientes y logs.
    public string ErrorCode { get; }
    // Título legible del error, independiente del detalle.
    public string ErrorTitle { get; }

    // Parámetro errorCode: Código de error estable.
    // Parámetro errorTitle: Título mostrado al cliente.
    // Parámetro statusCode: Código de estado HTTP.
    // Parámetro detail: Mensaje detallado (puede ser nulo).
    // Parámetro innerException: Excepción interna si aplica.
    protected AppException(
        string errorCode,
        string errorTitle,
        int statusCode,
        string? detail = null,
        Exception? innerException = null
    ) : base(detail, innerException) {
        ErrorCode = errorCode;
        ErrorTitle = errorTitle;
        StatusCode = statusCode;
    }
}
