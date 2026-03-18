using ApiMovies.Models.Dtos;
namespace ApiMovies.Interfaces.Services;

public interface IMovieService
{
    IEnumerable<MovieDto> GetMovies(string? search = null);
    IEnumerable<MovieDto> GetMoviesByCategory(int categoryId, string? search = null);
    MovieDto GetMovie(int movieId);
    MovieDto CreateMovie(MovieCreateDto movieDto);
    Task<MovieDto> UpdateMovie(int movieId, MovieUpdateDto movieDto);
    Task<MovieDto> ReplaceMovie(int movieId, MovieUpdateDto movieDto);
    MovieDto DeleteMovie(int movieId);
}