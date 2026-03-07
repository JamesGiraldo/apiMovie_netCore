using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos;

public class CategoryCreateDto
{
    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;
}