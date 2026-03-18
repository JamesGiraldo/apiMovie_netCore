using ApiMovies.Common.Constants;
using ApiMovies.Common.Exceptions;
using ApiMovies.Interfaces.Repositories;
using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace ApiMovies.Repositories;

public class RoleRepository : IRoleRepository {
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleRepository(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    ) {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task EnsureDefaultRoles() {
        if (!await _roleManager.RoleExistsAsync(RoleNames.Admin)) {
            await _roleManager.CreateAsync(new IdentityRole(RoleNames.Admin));
        }

        if (!await _roleManager.RoleExistsAsync(RoleNames.Registered)) {
            await _roleManager.CreateAsync(new IdentityRole(RoleNames.Registered));
        }
    }

    public async Task<bool> RoleExists(string roleName) {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public Task<IReadOnlyCollection<RoleDto>> GetRoles() {
        IReadOnlyCollection<RoleDto> roles = _roleManager.Roles
            .OrderBy(r => r.Name)
            .Select(r => new RoleDto {
                Id = r.Id,
                Name = r.Name ?? string.Empty,
                NormalizedName = r.NormalizedName ?? string.Empty
            })
            .ToArray();

        return Task.FromResult(roles);
    }

    public Task<RoleDto?> GetRoleById(string roleId) {
        var role = _roleManager.Roles
            .Where(r => r.Id == roleId)
            .Select(r => new RoleDto {
                Id = r.Id,
                Name = r.Name ?? string.Empty,
                NormalizedName = r.NormalizedName ?? string.Empty
            })
            .FirstOrDefault();

        return Task.FromResult(role);
    }

    public async Task<IList<string>> GetUserRoles(User user) {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task AddUserToRole(User user, string roleName) {
        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded) {
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            throw new InfrastructureException($"Could not add user to role '{roleName}'. {errors}");
        }
    }
}
