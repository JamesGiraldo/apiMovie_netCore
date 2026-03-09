using ApiMovies.Models;
using ApiMovies.Repositories.IRepository;
using ApiMovies.Data;

namespace ApiMovies.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context) {
        _context = context;
    }

    public ICollection<Category> GetCategories() {
        return _context.Categories.OrderBy(c => c.Name).ToList();
    }

    public bool CategoryExists(int categoryId) {
        return _context.Categories.Any(c => c.Id == categoryId);
    }

    public bool ExistsCategoryName(string name) {
        bool value = _context.Categories.Any(c => c.Name.Trim().ToLower() == name.Trim().ToLower());
        return value;
    }

    public Category? GetCategory(int categoryId) {
        return _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
    }

    public bool Save() {
        return _context.SaveChanges() > 0;
    }

    public bool CreateCategory(Category category) {
        category.CreatedAt = DateTime.UtcNow;
        _context.Add(category);
        return Save();
    }

    public bool UpdateCategory(Category category) {
        category.CreatedAt = DateTime.UtcNow;
        // Arreglar problema del put
        var categoryExists = _context.Categories.Find(category.Id);
        if (categoryExists != null) {
            _context.Entry(categoryExists).CurrentValues.SetValues(category);
        } else {
            _context.Add(category);
        }

        return Save();
    }

    public bool DeleteCategory(int categoryId) {
        var category = GetCategory(categoryId);
        if (category is null) {
            return false;
        }

        _context.Remove(category);
        return Save();
    }
}