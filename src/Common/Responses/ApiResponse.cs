namespace ApiMovies.Common.Responses;

// Envelope JSON uniforme para respuestas HTTP: título, código, bandera de éxito, detalle opcional y carga útil.
public class ApiResponse
{
    // Título breve del resultado (éxito o error).
    public string Title { get; set; } = string.Empty;
    // Código HTTP devuelto al cliente.
    public int Status { get; set; }
    // Indica si la operación fue exitosa.
    public bool Success { get; set; }
    // Mensaje adicional o traza amigable cuando aplica.
    public string? Detail { get; set; }
    // Datos de negocio, listas paginadas o errores de validación estructurados.
    public object? Data { get; set; }

    // Construye una respuesta de éxito con el código HTTP indicado.
    public static ApiResponse Ok(
        string title,
        int status = StatusCodes.Status200OK,
        object? data = null,
        string? detail = null
    ) {
        return new ApiResponse {
            Title = title,
            Status = status,
            Success = true,
            Detail = detail,
            Data = data
        };
    }

    // Construye una respuesta de fallo (validación, autorización, etc.).
    public static ApiResponse Fail(
        string title,
        int status,
        string? detail = null,
        object? data = null
    ) {
        return new ApiResponse {
            Title = title,
            Status = status,
            Success = false,
            Detail = detail,
            Data = data
        };
    }
}
