using System.ComponentModel.DataAnnotations;
using ApiMovies.Models.Enums;

namespace ApiMovies.Models.Dtos;

// Actualización de película (PATCH/PUT multipart); Id validado contra la ruta en servicio.
public class MovieUpdateDto
{
    // Opcional en ruta; si viene &gt; 0 debe coincidir con el id de la URL.
    public int Id { get; set; }

    // Nuevo título; unicidad verificada salvo que sea el mismo registro.
    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Duration { get; set; } = string.Empty;

    public TypeClassification TypeClassification { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    // Nueva imagen; si se envía reemplaza la anterior tras persistir.
    public IFormFile? Image { get; set; }
}
