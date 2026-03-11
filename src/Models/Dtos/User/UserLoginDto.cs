using System.ComponentModel.DataAnnotations;
namespace ApiMovies.Models.Dtos;

public class UserLoginDto {

    [MaxLength(100, ErrorMessage = "The username must be less than 100 characters")]
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "The email is not valid")]
    [MaxLength(100, ErrorMessage = "The email must be less than 100 characters")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "The password is required")]
    [MaxLength(50, ErrorMessage = "The password must be less than 50 characters")]
    public string Password { get; set; } = string.Empty;

}
