using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

public interface IMovieRepository
{
    Task<ICollection<Movie>> GetMovies();

    Task<ICollection<Movie>> GetMoviesByCategory(int categoryId, string? search = null);

    Task<ICollection<Movie>> SearchMovies(string name);

    Task<Movie?> GetMovie(int movieId);

    Task<bool> MovieExists(int movieId);

    Task<bool> ExistsMovieName(string name);

    Task<bool> CreateMovie(Movie movie);

    Task<bool> UpdateMovie(Movie movie);

    Task<bool> DeleteMovie(int movieId);
}