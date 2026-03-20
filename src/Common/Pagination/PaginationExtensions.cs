using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Common.Pagination;

// Extensiones para materializar colecciones en memoria o consultas EF como PagedResult{T}.
public static class PaginationExtensions {
    // Pagina una secuencia ya materializada o enumerable (primero cuenta en memoria, luego Skip/Take).
    // Parámetro source: Origen de datos (idealmente lista si ya se cargó).
    // Parámetro paginationQuery: Parámetros de página; si es nulo se usan los valores por defecto.
    public static PagedResult<T> ToPagedResult<T>(
        this IEnumerable<T> source,
        PaginationQuery? paginationQuery
    ) {
        var normalizedQuery = (paginationQuery ?? new PaginationQuery()).Normalize();
        var sourceList = source as IList<T> ?? source.ToList();
        var totalCount = sourceList.Count;

        var items = sourceList
            .Skip((normalizedQuery.PageNumber - 1) * normalizedQuery.PageSize)
            .Take(normalizedQuery.PageSize)
            .ToList();

        return new PagedResult<T>(items, totalCount, normalizedQuery.PageNumber, normalizedQuery.PageSize);
    }

    // Pagina en base de datos: un COUNT y luego la página con Skip/Take asíncronos.
    // Parámetro source: Consulta EF Core sin ejecutar aún.
    // Parámetro paginationQuery: Parámetros de página.
    // Parámetro cancellationToken: Token de cancelación.
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> source,
        PaginationQuery? paginationQuery,
        CancellationToken cancellationToken = default
    ) {
        var normalizedQuery = (paginationQuery ?? new PaginationQuery()).Normalize();
        var totalCount = await source.CountAsync(cancellationToken);

        var items = await source
            .Skip((normalizedQuery.PageNumber - 1) * normalizedQuery.PageSize)
            .Take(normalizedQuery.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, normalizedQuery.PageNumber, normalizedQuery.PageSize);
    }
}
