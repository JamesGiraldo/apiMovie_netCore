using System.ComponentModel.DataAnnotations;
using ApiMovies.Models.enums;
namespace ApiMovies.Models.Dtos;

public class MovieDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Duration { get; set; } = string.Empty;

    public TypeClassification TypeClassification { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int CategoryId { get; set; }

    public CategoryDto? Category { get; set; }
}