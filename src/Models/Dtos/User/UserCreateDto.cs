using System.ComponentModel.DataAnnotations;
namespace ApiMovies.Models.Dtos;

public class UserCreateDto {

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

    [Required(ErrorMessage = "The password is required")]
    [MaxLength(50, ErrorMessage = "The password must be less than 50 characters")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "The role is required")]
    public string Role { get; set; } = string.Empty;
}
