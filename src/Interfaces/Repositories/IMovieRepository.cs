using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

// Acceso a datos de películas: consultas con inclusión de categoría, búsqueda y CRUD.
public interface IMovieRepository
{
    // Todas las películas con categoría cargada, ordenadas por nombre.
    Task<ICollection<Movie>> GetMovies();

    // Películas de una categoría con filtro opcional en nombre y descripción.
    Task<ICollection<Movie>> GetMoviesByCategory(int categoryId, string? search = null);

    // Búsqueda global por nombre o descripción.
    Task<ICollection<Movie>> SearchMovies(string name);

    // Lectura por id sin tracking (según implementación).
    Task<Movie?> GetMovie(int movieId);

    // Indica si existe una película con el id dado.
    Task<bool> MovieExists(int movieId);

    // Unicidad de título de película (comparación normalizada).
    Task<bool> ExistsMovieName(string name);

    // Inserta la entidad y confirma cambios.
    Task<bool> CreateMovie(Movie movie);

    // Actualiza la entidad rastreada o adjunta según implementación.
    Task<bool> UpdateMovie(Movie movie);

    // Elimina por id si existe.
    Task<bool> DeleteMovie(int movieId);
}
