using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

// Gestión de roles ASP.NET Identity: listado, asignación idempotente y consulta de roles por usuario.
public interface IRoleService {
    // Garantiza roles semilla (Admin, Registered) y devuelve todos los roles.
    Task<IReadOnlyCollection<RoleDto>> GetRoles();

    // Asigna el rol indicado por id al usuario si aún no lo tiene; devuelve el estado actualizado.
    Task<UserRolesDto> AssignRoleToUser(string userId, AssignUserRoleDto assignUserRoleDto);

    // Lista de nombres de rol del usuario.
    Task<IList<string>> GetUserRoles(string userId);

    // Si el rol existe y el usuario existe, agrega el rol solo cuando falta (sin duplicar).
    Task EnsureUserHasRole(string userId, string roleName);
}
