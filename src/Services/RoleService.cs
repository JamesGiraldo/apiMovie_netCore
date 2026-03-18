using ApiMovies.Common.Exceptions;
using ApiMovies.Interfaces.Repositories;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;

namespace ApiMovies.Services;

public class RoleService : IRoleService {
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;

    public RoleService(
        IRoleRepository roleRepository,
        IUserRepository userRepository
    ) {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyCollection<RoleDto>> GetRoles() {
        try {
            await _roleRepository.EnsureDefaultRoles();
            return await _roleRepository.GetRoles();
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            throw new InfrastructureException(
                "An unexpected error occurred while retrieving roles.",
                ex
            );
        }
    }

    public async Task<UserRolesDto> AssignRoleToUser(string userId, AssignUserRoleDto assignUserRoleDto) {
        try {
            ValidateAssignRoleRequest(userId, assignUserRoleDto);

            var role = await _roleRepository.GetRoleById(assignUserRoleDto.RoleId.Trim());
            if (role is null) {
                throw new NotFoundException($"Role with id {assignUserRoleDto.RoleId} was not found.");
            }

            await EnsureUserHasRole(userId, role.Name);
            var roles = await GetUserRoles(userId);
            var user = await _userRepository.GetUser(userId)
                ?? throw new NotFoundException($"User with id {userId} was not found.");

            return new UserRolesDto {
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Roles = roles.OrderBy(r => r).ToArray()
            };
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            throw new InfrastructureException(
                "An unexpected error occurred while assigning role to user.",
                ex
            );
        }
    }

    public async Task<IList<string>> GetUserRoles(string userId) {
        try {
            if (string.IsNullOrWhiteSpace(userId)) {
                throw new BadRequestException("userId is required.");
            }

            var user = await _userRepository.GetUser(userId);
            if (user is null) {
                throw new NotFoundException($"User with id {userId} was not found.");
            }

            return await _roleRepository.GetUserRoles(user);
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            throw new InfrastructureException(
                "An unexpected error occurred while retrieving user roles.",
                ex
            );
        }
    }

    public async Task EnsureUserHasRole(string userId, string roleName) {
        try {
            if (string.IsNullOrWhiteSpace(userId)) {
                throw new BadRequestException("userId is required.");
            }

            if (string.IsNullOrWhiteSpace(roleName)) {
                throw new BadRequestException("Role name is required.");
            }

            await _roleRepository.EnsureDefaultRoles();

            var normalizedRoleName = roleName.Trim();
            if (!await _roleRepository.RoleExists(normalizedRoleName)) {
                throw new NotFoundException($"Role '{normalizedRoleName}' was not found.");
            }

            var user = await _userRepository.GetUser(userId);
            if (user is null) {
                throw new NotFoundException($"User with id {userId} was not found.");
            }

            var currentRoles = await _roleRepository.GetUserRoles(user);
            if (!currentRoles.Contains(normalizedRoleName, StringComparer.OrdinalIgnoreCase)) {
                await _roleRepository.AddUserToRole(user, normalizedRoleName);
            }
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            throw new InfrastructureException(
                "An unexpected error occurred while assigning role to user.",
                ex
            );
        }
    }

    private static void ValidateAssignRoleRequest(string userId, AssignUserRoleDto assignUserRoleDto) {
        if (string.IsNullOrWhiteSpace(userId)) {
            throw new BadRequestException("userId is required.");
        }

        if (assignUserRoleDto is null) {
            throw new BadRequestException("Role payload is required.");
        }

        if (string.IsNullOrWhiteSpace(assignUserRoleDto.RoleId)) {
            throw new BadRequestException("Role id is required.");
        }
    }
}
