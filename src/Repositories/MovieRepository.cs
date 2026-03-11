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

    public ICollection<Movie> GetMovies() {
        return _db.Movie.OrderBy(m => m.Name).Include(c => c.Category)
        .ToList();
    }

    public ICollection<Movie> GetMoviesByCategory(int categoryId, string? search = null) {
        IQueryable<Movie> query = _db.Movie
            .Include(c => c.Category)
            .Where(m => m.CategoryId == categoryId);

        query = ApplySearchFilter(query, search);

        return query.ToList();
    }

    public IEnumerable<Movie> SearchMovies(string name) {
        IQueryable<Movie> query = _db.Movie;
        query = ApplySearchFilter(query, name);

        return query.ToList();
    }

    public Movie? GetMovie(int movieId) {
        return _db.Movie.AsNoTracking().Where(m => m.Id == movieId).FirstOrDefault();
    }

    public bool MovieExists(int movieId) {
        return _db.Movie.Any(m => m.Id == movieId);
    }

    public bool ExistsMovieName(string name) {
        return _db.Movie.Any(m => m.Name.Trim().ToLower() == name.Trim().ToLower());
    }

    public bool CreateMovie(Movie movie) {
        _db.Add(movie);
        return Save();
    }

    public bool UpdateMovie(Movie movie) {
        _db.Update(movie);
        return Save();
    }

    public bool DeleteMovie(int movieId) {
        var movie = GetMovie(movieId);
        if (movie is null) {
            return false;
        }

        _db.Remove(movie);
        return Save();
    }

    public bool Save() {
        return _db.SaveChanges() > 0;
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