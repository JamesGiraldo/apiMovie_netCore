using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface ICategoryService
{
    ServiceResult<IEnumerable<CategoryDto>> GetCategories(string? search = null);
    ServiceResult<CategoryDto> GetCategory(int categoryId);
    ServiceResult<CategoryDto> CreateCategory(CategoryCreateDto categoryDto);
    ServiceResult<CategoryDto> UpdateCategory(int categoryId, CategoryDto categoryDto);
    ServiceResult<CategoryDto> ReplaceCategory(int categoryId, CategoryDto categoryDto);
    ServiceResult<CategoryDto> DeleteCategory(int categoryId);
}
