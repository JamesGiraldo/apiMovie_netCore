using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface IRoleService {
    Task<IReadOnlyCollection<RoleDto>> GetRoles();
    Task<UserRolesDto> AssignRoleToUser(string userId, AssignUserRoleDto assignUserRoleDto);
    Task<IList<string>> GetUserRoles(string userId);
    Task EnsureUserHasRole(string userId, string roleName);
}
