using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface IMovieService
{
    IEnumerable<MovieDto> GetMovies(string? search = null);
    IEnumerable<MovieDto> GetMoviesByCategory(int categoryId, string? search = null);
    MovieDto GetMovie(int movieId);
    MovieDto CreateMovie(MovieCreateDto movieDto);
    MovieDto UpdateMovie(int movieId, MovieDto movieDto);
    MovieDto ReplaceMovie(int movieId, MovieDto movieDto);
    MovieDto DeleteMovie(int movieId);
}