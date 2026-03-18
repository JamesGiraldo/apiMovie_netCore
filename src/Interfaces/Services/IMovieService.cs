using ApiMovies.Models.Dtos;
using ApiMovies.Common.Pagination;
namespace ApiMovies.Interfaces.Services;

public interface IMovieService
{
    Task<PagedResult<MovieDto>> GetMovies(string? search = null, PaginationQuery? paginationQuery = null);
    Task<PagedResult<MovieDto>> GetMoviesByCategory(int categoryId, string? search = null, PaginationQuery? paginationQuery = null);
    Task<MovieDto> GetMovie(int movieId);
    Task<MovieDto> CreateMovie(MovieCreateDto movieDto);
    Task<MovieDto> UpdateMovie(int movieId, MovieUpdateDto movieDto);
    Task<MovieDto> ReplaceMovie(int movieId, MovieUpdateDto movieDto);
    Task<MovieDto> DeleteMovie(int movieId);
}