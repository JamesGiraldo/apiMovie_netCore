using System.ComponentModel.DataAnnotations;
using ApiMovies.Models.enums;

namespace ApiMovies.Models.Dtos;

public class MovieCreateDto
{
    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Duration { get; set; } = string.Empty;

    public TypeClassification TypeClassification { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public int CategoryId { get; set; }
}