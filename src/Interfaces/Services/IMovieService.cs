using ApiMovies.Models.Dtos;
namespace ApiMovies.Interfaces.Services;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetMovies(string? search = null);
    Task<IEnumerable<MovieDto>> GetMoviesByCategory(int categoryId, string? search = null);
    Task<MovieDto> GetMovie(int movieId);
    Task<MovieDto> CreateMovie(MovieCreateDto movieDto);
    Task<MovieDto> UpdateMovie(int movieId, MovieUpdateDto movieDto);
    Task<MovieDto> ReplaceMovie(int movieId, MovieUpdateDto movieDto);
    Task<MovieDto> DeleteMovie(int movieId);
}