using ApiMovies.Models.Entities;
using ApiMovies.Interfaces.Repositories;
using ApiMovies.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context) {
        _context = context;
    }

    public ICollection<Category> GetCategories(string? search = null) {
        IQueryable<Category> query = _context.Category;
        query = ApplySearchFilter(query, search);
        return query.OrderBy(c => c.Name).ToList();
    }

    public bool CategoryExists(int categoryId) {
        return _context.Category.Any(c => c.Id == categoryId);
    }

    public bool ExistsCategoryName(string name) {
        bool value = _context.Category.Any(c => c.Name.Trim().ToLower() == name.Trim().ToLower());
        return value;
    }

    public Category? GetCategory(int categoryId) {
        return _context.Category.Where(c => c.Id == categoryId).FirstOrDefault();
    }

    public bool Save() {
        return _context.SaveChanges() > 0;
    }

    public bool CreateCategory(Category category) {
        _context.Add(category);
        return Save();
    }

    public bool UpdateCategory(Category category) {
        var categoryExists = _context.Category.Find(category.Id);
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

    private static IQueryable<Category> ApplySearchFilter(IQueryable<Category> query, string? search) {
        if (string.IsNullOrWhiteSpace(search)) return query;

        var pattern = $"%{search.Trim()}%";
        return query.Where(c => EF.Functions.ILike(c.Name, pattern));
    }
}