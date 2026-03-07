using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos;

public class CategoryDto
{
    public int id { get; set; }

    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;

    public DateTime createdAt { get; set; }
}