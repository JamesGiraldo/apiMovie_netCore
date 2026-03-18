using ApiMovies.Models.Entities;
using ApiMovies.Interfaces.Repositories;
using ApiMovies.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _db;

    public MovieRepository(ApplicationDbContext context) {
        _db = context;
    }

    public async Task<ICollection<Movie>> GetMovies() {
        return await _db.Movie
            .OrderBy(m => m.Name)
            .Include(c => c.Category)
            .ToListAsync();
    }

    public async Task<ICollection<Movie>> GetMoviesByCategory(int categoryId, string? search = null) {
        IQueryable<Movie> query = _db.Movie
            .Include(c => c.Category)
            .Where(m => m.CategoryId == categoryId);

        query = ApplySearchFilter(query, search);

        return await query.ToListAsync();
    }

    public async Task<ICollection<Movie>> SearchMovies(string name) {
        IQueryable<Movie> query = _db.Movie;
        query = ApplySearchFilter(query, name);

        return await query.ToListAsync();
    }

    public async Task<Movie?> GetMovie(int movieId) {
        return await _db.Movie
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == movieId);
    }

    public async Task<bool> MovieExists(int movieId) {
        return await _db.Movie.AnyAsync(m => m.Id == movieId);
    }

    public async Task<bool> ExistsMovieName(string name) {
        return await _db.Movie.AnyAsync(m => m.Name.Trim().ToLower() == name.Trim().ToLower());
    }

    public async Task<bool> CreateMovie(Movie movie) {
        _db.Add(movie);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateMovie(Movie movie) {
        _db.Update(movie);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteMovie(int movieId) {
        var movie = await GetMovie(movieId);
        if (movie is null) {
            return false;
        }

        _db.Remove(movie);
        return await _db.SaveChangesAsync() > 0;
    }

    private static IQueryable<Movie> ApplySearchFilter(IQueryable<Movie> query, string? search) {
        if (string.IsNullOrWhiteSpace(search)) {
            return query;
        }

        var pattern = $"%{search.Trim()}%";
        return query.Where(m =>
            EF.Functions.ILike(m.Name, pattern) ||
            EF.Functions.ILike(m.Description, pattern));
    }
}