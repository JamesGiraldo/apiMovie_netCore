using ApiMovies.Models.Dtos;
using ApiMovies.Common.Pagination;
namespace ApiMovies.Interfaces.Services;

// Casos de uso de películas: búsqueda, filtrado por categoría, CRUD y gestión de imagen en almacenamiento.
public interface IMovieService
{
    // Todas las películas o búsqueda por nombre/descripción, paginado en memoria.
    Task<PagedResult<MovieDto>> GetMovies(string? search = null, PaginationQuery? paginationQuery = null);

    // Películas de una categoría con filtro de texto opcional.
    Task<PagedResult<MovieDto>> GetMoviesByCategory(int categoryId, string? search = null, PaginationQuery? paginationQuery = null);

    // Detalle por id con URLs de imagen resueltas.
    Task<MovieDto> GetMovie(int movieId);

    // Crea película y, si viene imagen, la sube y persiste la ruta/URL.
    Task<MovieDto> CreateMovie(MovieCreateDto movieDto);

    // Actualiza campos y opcionalmente reemplaza imagen (subida + borrado de la anterior).
    Task<MovieDto> UpdateMovie(int movieId, MovieUpdateDto movieDto);

    // PUT semántico; implementación actual delega en UpdateMovie.
    Task<MovieDto> ReplaceMovie(int movieId, MovieUpdateDto movieDto);

    // Elimina registro y borra archivo asociado en storage si existe.
    Task<MovieDto> DeleteMovie(int movieId);
}
