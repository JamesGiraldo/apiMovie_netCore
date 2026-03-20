using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Entities;

// Categoría de catálogo de películas con nombre único (regla aplicada en capa de servicio).
public class Category
{
    [Key]
    public int Id { get; set; }

    // Nombre mostrado y usado en búsquedas ILIKE.
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}