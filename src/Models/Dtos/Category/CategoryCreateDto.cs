using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos;

// Cuerpo JSON para crear categoría (solo nombre validado en modelo y en servicio por unicidad).
public class CategoryCreateDto
{
    // Nombre obligatorio; máximo 100 caracteres.
    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;
}
