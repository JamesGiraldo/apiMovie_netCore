using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ApiMovies.Models.Dtos;

// Perfil de usuario para administración: datos básicos, avatar enriquecido y roles efectivos.
public class UserDto {

    [Required(ErrorMessage = "The id is required")]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The username is required")]
    [MaxLength(100, ErrorMessage = "The username must be less than 100 characters")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "The email is required")]
    [EmailAddress(ErrorMessage = "The email is not valid")]
    [MaxLength(100, ErrorMessage = "The email must be less than 100 characters")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "The phone number is not valid")]
    [Column(TypeName = "varchar(15)")]
    [MaxLength(15, ErrorMessage = "The phone number must be less than 15 characters")]
    public string? PhoneNumber { get; set; } = null;

    // URL pública o almacenada del avatar.
    public string? Image { get; set; }

    // URL firmada para previsualización.
    public string? ImageUrl { get; set; } = null;
    public string[] Roles { get; set; } = Array.Empty<string>();
}
