namespace ApiMovies.Common.Pagination;

// Resultado paginado: ítems de la página actual más metadatos para navegación en el cliente.
// <typeparam name="T">Tipo de cada elemento en la página.</typeparam>
public class PagedResult<T> {
    // Registros de la página solicitada.
    public IReadOnlyCollection<T> Items { get; set; } = Array.Empty<T>();
    // Página actual (1-based).
    public int PageNumber { get; set; }
    // Tamaño de página aplicado.
    public int PageSize { get; set; }
    // Total de registros que cumplen el filtro (todas las páginas).
    public int TotalCount { get; set; }
    // Número total de páginas según TotalCount y PageSize.
    public int TotalPages { get; set; }
    // Indica si existe página anterior.
    public bool HasPreviousPage { get; set; }
    // Indica si existe página siguiente.
    public bool HasNextPage { get; set; }

    public PagedResult() { }

    // Parámetro items: Elementos ya recortados a la ventana actual.
    // Parámetro totalCount: Conteo total sin paginar.
    // Parámetro pageNumber: Página actual.
    // Parámetro pageSize: Tamaño de página.
    public PagedResult(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize) {
        Items = items;
        TotalCount = Math.Max(totalCount, 0);
        PageNumber = Math.Max(pageNumber, PaginationQuery.DefaultPageNumber);
        PageSize = Math.Max(pageSize, PaginationQuery.DefaultPageSize);
        TotalPages = TotalCount == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        HasPreviousPage = PageNumber > 1 && TotalPages > 0;
        HasNextPage = TotalPages > 0 && PageNumber < TotalPages;
    }
}
