namespace ApiMovies.Common.Pagination;

public class PagedResult<T> {
    public IReadOnlyCollection<T> Items { get; set; } = Array.Empty<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    public PagedResult() { }

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
