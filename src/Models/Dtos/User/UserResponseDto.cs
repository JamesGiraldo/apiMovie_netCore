namespace ApiMovies.Models.Dtos;

public class UserResponseDto {

    public UserInfoDto User { get; set; } = new UserInfoDto();
    public DateTime Expiration { get; set; } = DateTime.UtcNow;
    public string Token { get; set; } = string.Empty;

}
