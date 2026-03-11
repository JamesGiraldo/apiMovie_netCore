using ApiMovies.Models.Dtos;

namespace ApiMovies.Services.IService;

public interface IMovieService
{
    public ServiceResult<IEnumerable<MovieDto>> GetMovies(string? search = null);
    public ServiceResult<IEnumerable<MovieDto>> GetMoviesByCategory(int categoryId, string? search = null);
    public ServiceResult<MovieDto> GetMovie(int movieId);
    public ServiceResult<MovieDto> CreateMovie(MovieCreateDto movieDto);
    public ServiceResult<MovieDto> UpdateMovie(int movieId, MovieDto movieDto);
    public ServiceResult<MovieDto> ReplaceMovie(int movieId, MovieDto movieDto);
    public ServiceResult<MovieDto> DeleteMovie(int movieId);
}