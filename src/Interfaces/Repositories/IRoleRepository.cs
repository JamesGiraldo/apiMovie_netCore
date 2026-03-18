using ApiMovies.Models.Entities;
using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Repositories;

public interface IRoleRepository {
    Task EnsureDefaultRoles();
    Task<bool> RoleExists(string roleName);
    Task<IReadOnlyCollection<RoleDto>> GetRoles();
    Task<RoleDto?> GetRoleById(string roleId);
    Task<IList<string>> GetUserRoles(User user);
    Task AddUserToRole(User user, string roleName);
}
