using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface ICategoryService
{
    IEnumerable<CategoryDto> GetCategories(string? search = null);
    CategoryDto GetCategory(int categoryId);
    CategoryDto CreateCategory(CategoryCreateDto categoryDto);
    CategoryDto UpdateCategory(int categoryId, CategoryDto categoryDto);
    CategoryDto ReplaceCategory(int categoryId, CategoryDto categoryDto);
    CategoryDto DeleteCategory(int categoryId);
}
