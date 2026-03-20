namespace ApiMovies.Common.Constants;

// Nombres de roles de ASP.NET Identity usados en políticas [Authorize(Roles = ...)] y semillas de datos.
public static class RoleNames {
    // Rol con privilegios de administración del catálogo y usuarios.
    public const string Admin = "Admin";
    // Rol asignado por defecto a usuarios registrados vía API.
    public const string Registered = "Registered";
}
