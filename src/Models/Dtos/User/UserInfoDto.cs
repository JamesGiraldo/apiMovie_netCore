namespace ApiMovies.Models.Dtos;

// Subconjunto de datos del usuario embebido en respuestas de autenticación y claims del token.
public class UserInfoDto {

    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Image { get; set; } = null;

    public string? ImageUrl { get; set; } = string.Empty;

    // Roles efectivos al momento de emitir el token.
    public List<string> Roles { get; set; } = new List<string>();

}
