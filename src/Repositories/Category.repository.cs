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

    public bool UpdateCategory(Category category) {
        category.CreatedAt = DateTime.Now;
        _context.Update(category);
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

    public bool CreateCategory(Category category) {
        category.CreatedAt = DateTime.Now;
        _context.Add(category);
        return Save();
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

    public ICollection<Category> GetCategories() {
        return _context.Categories.OrderBy(c => c.Name).ToList();
    }

    public bool Save() {
        return _context.SaveChanges() > 0;
    }
}