using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
}