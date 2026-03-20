using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ApiMovies.Models.Dtos;

// Registro de usuario con contraseña e imagen opcional (formulario multipart).
public class UserCreateDto {

    [Required(ErrorMessage = "The name is required")]
    [MaxLength(100, ErrorMessage = "The name must be less than 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The last name is required")]
    [MaxLength(100, ErrorMessage = "The last name must be less than 100 characters")]
    public string LastName { get; set; } = string.Empty;

    // Nombre de usuario único entre cuentas activas.
    [Required(ErrorMessage = "The username is required")]
    [MaxLength(100, ErrorMessage = "The username must be less than 100 characters")]
    public string UserName { get; set; } = string.Empty;

    // Correo único; se normaliza en persistencia.
    [Required(ErrorMessage = "The email is required")]
    [EmailAddress(ErrorMessage = "The email is not valid")]
    [MaxLength(100, ErrorMessage = "The email must be less than 100 characters")]
    public string Email { get; set; } = string.Empty;

    // Teléfono opcional; formato validado y longitud acotada.
    [Phone(ErrorMessage = "The phone number is not valid")]
    [Column(TypeName = "varchar(15)")]
    [MaxLength(15, ErrorMessage = "The phone number must be less than 15 characters")]
    public string PhoneNumber { get; set; } = string.Empty;

    // Foto de perfil opcional.
    public IFormFile? Image { get; set; } = null!;

    // Contraseña en claro solo en tránsito; Identity almacena hash.
    [Required(ErrorMessage = "The password is required")]
    [MaxLength(50, ErrorMessage = "The password must be less than 50 characters")]
    public string Password { get; set; } = string.Empty;

}
