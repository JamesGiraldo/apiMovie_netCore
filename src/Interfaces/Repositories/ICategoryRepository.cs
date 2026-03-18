using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<ICollection<Category>> GetCategories(string? search = null);

    Task<Category?> GetCategory(int CategoryId);

    Task<bool> CategoryExists(int categoryId);

    Task<bool> ExistsCategoryName(string name);

    Task<bool> CreateCategory(Category category);

    Task<bool> UpdateCategory(Category category);

    Task<bool> DeleteCategory(int CategoryId);
}