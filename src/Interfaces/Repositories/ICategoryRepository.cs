using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

// Acceso a datos de categorías en EF Core (consultas, existencias y persistencia).
public interface ICategoryRepository
{
    // Listado ordenado por nombre con filtro de búsqueda opcional (ILIKE).
    Task<ICollection<Category>> GetCategories(string? search = null);

    // Obtiene por clave primaria o null.
    Task<Category?> GetCategory(int CategoryId);

    // Indica si existe el id.
    Task<bool> CategoryExists(int categoryId);

    // Unicidad de nombre ignorando mayúsculas y espacios extremos.
    Task<bool> ExistsCategoryName(string name);

    // Inserta y confirma transacción; devuelve si SaveChanges afectó filas.
    Task<bool> CreateCategory(Category category);

    // Actualiza copiando valores sobre la entidad rastreada.
    Task<bool> UpdateCategory(Category category);

    // Elimina físicamente si existe.
    Task<bool> DeleteCategory(int CategoryId);
}
