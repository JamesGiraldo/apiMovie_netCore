using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos;

// Representación de categoría en lecturas y actualizaciones (incluye id y fecha de creación).
public class CategoryDto
{
    public int Id { get; set; }

    // Nombre obligatorio en actualizaciones; debe coincidir con reglas de ruta vs cuerpo en PATCH/PUT.
    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;

    // Fecha de creación original (convención de propiedad en minúsculas heredada del contrato API).
    public DateTime createdAt { get; set; }
}
