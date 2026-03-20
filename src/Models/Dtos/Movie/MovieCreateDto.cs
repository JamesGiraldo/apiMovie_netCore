using System.ComponentModel.DataAnnotations;
using ApiMovies.Models.Enums;

namespace ApiMovies.Models.Dtos;

// Alta de película vía multipart/form-data: metadatos más archivo de imagen opcional.
public class MovieCreateDto
{
    // Título; debe ser único (validación en servicio).
    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Duration { get; set; } = string.Empty;

    public TypeClassification TypeClassification { get; set; }

    // Ruta interna opcional; normalmente se rellena tras subir Image.
    public string? FilePath { get; set; } = null;

    // Categoría a la que pertenece la película.
    public int CategoryId { get; set; }

    // Archivo de póster; tipos permitidos definidos en almacenamiento.
    public IFormFile? Image { get; set; }
}
