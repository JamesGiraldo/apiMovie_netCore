using System.ComponentModel.DataAnnotations;
namespace ApiMovies.Models.Dtos;

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

    public string[] Roles { get; set; } = Array.Empty<string>();
}
