using ApiMovies.Models.Dtos;
using ApiMovies.Common.Pagination;

namespace ApiMovies.Interfaces.Services;

public interface ICategoryService
{
    Task<PagedResult<CategoryDto>> GetCategories(string? search = null, PaginationQuery? paginationQuery = null);
    Task<CategoryDto> GetCategory(int categoryId);
    Task<CategoryDto> CreateCategory(CategoryCreateDto categoryDto);
    Task<CategoryDto> UpdateCategory(int categoryId, CategoryDto categoryDto);
    Task<CategoryDto> ReplaceCategory(int categoryId, CategoryDto categoryDto);
    Task<CategoryDto> DeleteCategory(int categoryId);
}
