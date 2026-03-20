namespace ApiMovies.Common.Exceptions;

// Error no esperado en infraestructura (BD, almacenamiento, Identity, etc.); HTTP 500.
// Suele envolver una Exception interna para diagnóstico en logs.
public class InfrastructureException : AppException
{
    // Parámetro detail: Descripción del fallo para el cliente o logs.
    // Parámetro innerException: Excepción original (stack trace preservado).
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
