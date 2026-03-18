namespace ApiMovies.Models.Dtos;

public class UserRolesDto {
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();
}
