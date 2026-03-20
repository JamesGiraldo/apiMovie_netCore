using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiMovies.Models.Enums;

namespace ApiMovies.Models.Entities;

// Película persistida con referencia obligatoria a Category y ruta/URL de póster en almacenamiento.
public class Movie
{
    [Key]
    public int Id { get; set; }

    // Título único en reglas de negocio (validado en servicio).
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Duration { get; set; } = string.Empty;

    // Clasificación por edad (valores del enum TypeClassification).
    public TypeClassification TypeClassification { get; set; }

    // URL pública o clave relativa del archivo en S3 según lo guardado tras la subida.
    public string FilePath { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Clave foránea a la categoría contenedora.
    [Required]
    public int CategoryId { get; set; }

    // Navegación a categoría (Include en repositorio).
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
}