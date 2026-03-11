using ApiMovies.Models;

namespace ApiMovies.Interfaces.Repositories;

public interface ICategoryRepository
{
    ICollection<Category> GetCategories(string? search = null);

    Category? GetCategory(int CategoryId);

    bool CategoryExists(int categoryId);

    bool ExistsCategoryName(string name);

    bool CreateCategory(Category category);

    bool UpdateCategory(Category category);

    bool DeleteCategory(int CategoryId);

    bool Save();
}