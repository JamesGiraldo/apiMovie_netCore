using System.ComponentModel.DataAnnotations;
namespace ApiMovies.Models.Dtos;

public class UserResponseDto {

    public string Token { get; set; } = string.Empty;
    public UserInfoDto User { get; set; } = new UserInfoDto();
    public DateTime Expiration { get; set; } = DateTime.UtcNow;

}
