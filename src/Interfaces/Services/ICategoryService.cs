using ApiMovies.Models.Dtos;
using ApiMovies.Common.Pagination;

namespace ApiMovies.Interfaces.Services;

// Casos de uso de categorías: listado paginado, CRUD y reglas de nombre único.
public interface ICategoryService
{
    // Listado con búsqueda opcional por nombre (ILIKE) y paginación en memoria tras cargar el conjunto.
    Task<PagedResult<CategoryDto>> GetCategories(string? search = null, PaginationQuery? paginationQuery = null);

    // Obtiene una categoría por id o lanza si no existe.
    Task<CategoryDto> GetCategory(int categoryId);

    // Crea categoría validando nombre único.
    Task<CategoryDto> CreateCategory(CategoryCreateDto categoryDto);

    // Actualización parcial lógica (PATCH): mismo flujo que reemplazo con validación de ruta vs cuerpo.
    Task<CategoryDto> UpdateCategory(int categoryId, CategoryDto categoryDto);

    // Reemplazo completo (PUT); en la implementación actual delega en UpdateCategory.
    Task<CategoryDto> ReplaceCategory(int categoryId, CategoryDto categoryDto);

    // Elimina por id y devuelve el DTO de lo eliminado.
    Task<CategoryDto> DeleteCategory(int categoryId);
}
