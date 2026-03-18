using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategories(string? search = null);
    Task<CategoryDto> GetCategory(int categoryId);
    Task<CategoryDto> CreateCategory(CategoryCreateDto categoryDto);
    Task<CategoryDto> UpdateCategory(int categoryId, CategoryDto categoryDto);
    Task<CategoryDto> ReplaceCategory(int categoryId, CategoryDto categoryDto);
    Task<CategoryDto> DeleteCategory(int categoryId);
}
