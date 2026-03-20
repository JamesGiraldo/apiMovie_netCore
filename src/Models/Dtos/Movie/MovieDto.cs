using System.ComponentModel.DataAnnotations;
using ApiMovies.Models.Enums;
namespace ApiMovies.Models.Dtos;

// Película expuesta al cliente con URLs de imagen resueltas y categoría anidada opcional.
public class MovieDto
{
    public int Id { get; set; }

    // Título requerido en respuestas que reflejan estado persistido.
    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Duration { get; set; } = string.Empty;

    public TypeClassification TypeClassification { get; set; }

    // Identificador almacenado (URL o clave) antes de enriquecer con firmas.
    public string FilePath { get; set; } = string.Empty;

    // URL firmada o pública para descarga directa si se expone en el futuro.
    public string FileDownloadUrl { get; set; } = string.Empty;

    // Vista previa firmada para incrustar en cliente.
    public string ImageUrl { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int CategoryId { get; set; }

    public CategoryDto? Category { get; set; }
}
