using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Common.Pagination;

public class PaginationQuery {
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 100;

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = DefaultPageNumber;

    [Range(1, MaxPageSize)]
    public int PageSize { get; set; } = DefaultPageSize;

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
