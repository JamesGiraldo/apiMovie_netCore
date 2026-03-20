namespace ApiMovies.Models.Dtos;

// Rol de Identity proyectado para listados y asignaciones (id, nombre y nombre normalizado).
public class RoleDto {
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
}
