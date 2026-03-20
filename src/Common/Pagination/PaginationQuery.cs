using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Common.Pagination;

// Parámetros de paginación desde query string; Normalize aplica límites seguros antes de consultar la BD.
public class PaginationQuery {
    // Número de página por defecto (1-based).
    public const int DefaultPageNumber = 1;
    // Tamaño de página por defecto cuando el cliente no lo envía.
    public const int DefaultPageSize = 10;
    // Tope superior del tamaño de página para evitar consultas demasiado grandes.
    public const int MaxPageSize = 100;

    // Página actual (≥ 1).
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = DefaultPageNumber;

    // Elementos por página (entre 1 y MaxPageSize).
    [Range(1, MaxPageSize)]
    public int PageSize { get; set; } = DefaultPageSize;

    // Corrige valores fuera de rango (página &lt; 1, tamaño inválido o mayor al máximo).
    // Retorna: La misma instancia mutada, para encadenar llamadas.
    public PaginationQuery Normalize() {
        if (PageNumber < 1) {
            PageNumber = DefaultPageNumber;
        }

        if (PageSize < 1) {
            PageSize = DefaultPageSize;
        }

        if (PageSize > MaxPageSize) {
            PageSize = MaxPageSize;
        }

        return this;
    }
}
