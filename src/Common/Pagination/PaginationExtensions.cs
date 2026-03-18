using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Common.Pagination;

public static class PaginationExtensions {
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
