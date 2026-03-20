namespace ApiMovies.Models.Dtos;

// Vista de usuario con la lista actual de roles tras una asignación o consulta.
public class UserRolesDto {
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();
}
