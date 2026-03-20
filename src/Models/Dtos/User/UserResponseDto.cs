namespace ApiMovies.Models.Dtos;

// Respuesta estándar tras login o registro: perfil enriquecido, JWT y caducidad declarada (alineada al token).
public class UserResponseDto {

    public UserInfoDto User { get; set; } = new UserInfoDto();
    public DateTime Expiration { get; set; } = DateTime.UtcNow;
    public string Token { get; set; } = string.Empty;

}
