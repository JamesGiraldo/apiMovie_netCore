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

    public async Task<ICollection<Category>> GetCategories(string? search = null) {
        IQueryable<Category> query = _context.Category;
        query = ApplySearchFilter(query, search);
        return await query.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<bool> CategoryExists(int categoryId) {
        return await _context.Category.AnyAsync(c => c.Id == categoryId);
    }

    public async Task<bool> ExistsCategoryName(string name) {
        bool value = await _context.Category.AnyAsync(c => c.Name.Trim().ToLower() == name.Trim().ToLower());
        return value;
    }

    public async Task<Category?> GetCategory(int categoryId) {
        return await _context.Category.FirstOrDefaultAsync(c => c.Id == categoryId);
    }

    public async Task<bool> CreateCategory(Category category) {
        _context.Add(category);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateCategory(Category category) {
        var categoryExists = await _context.Category.FindAsync(category.Id);
        if (categoryExists is null) {
            return false;
        }

        _context.Entry(categoryExists).CurrentValues.SetValues(category);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteCategory(int categoryId) {
        var category = await GetCategory(categoryId);
        if (category is null) {
            return false;
        }

        _context.Remove(category);
        return await _context.SaveChangesAsync() > 0;
    }

    private static IQueryable<Category> ApplySearchFilter(IQueryable<Category> query, string? search) {
        if (string.IsNullOrWhiteSpace(search)) return query;

        var pattern = $"%{search.Trim()}%";
        return query.Where(c => EF.Functions.ILike(c.Name, pattern));
    }
}