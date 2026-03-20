using ApiMovies.Models.Entities;
using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Repositories;

// Roles Identity y relación usuario-rol: semillas, consultas y alta de membresía.
public interface IRoleRepository {
    // Crea roles por defecto si no existen en la base.
    Task EnsureDefaultRoles();

    Task<bool> RoleExists(string roleName);

    // Todos los roles ordenados por nombre.
    Task<IReadOnlyCollection<RoleDto>> GetRoles();

    Task<RoleDto?> GetRoleById(string roleId);

    // Nombres de roles del usuario.
    Task<IList<string>> GetUserRoles(User user);

    // Agrega membresía; la implementación falla con error de infraestructura si Identity rechaza la operación.
    Task AddUserToRole(User user, string roleName);
}
