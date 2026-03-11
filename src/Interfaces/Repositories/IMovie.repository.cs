using ApiMovies.Models;

namespace ApiMovies.Interfaces.Repositories;

public interface IMovieRepository
{
    ICollection<Movie> GetMovies();

    ICollection<Movie> GetMoviesByCategory(int categoryId, string? search = null);

    IEnumerable<Movie> SearchMovies(string name);

    Movie? GetMovie(int movieId);

    bool MovieExists(int movieId);

    bool ExistsMovieName(string name);

    bool CreateMovie(Movie movie);

    bool UpdateMovie(Movie movie);

    bool DeleteMovie(int movieId);

    bool Save();
}