using ApiMovies.Interfaces.Repositories;
using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;
using ApiMovies.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
namespace ApiMovies.Repositories;

public class AuthRepository : IAuthRepository {

    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthRepository(
        IUserRepository userRepository,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    ) {
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<User?> GetUserForLogin(string? userName, string? email) {
        return await _userRepository.GetByUserNameOrEmail(userName, email);
    }

    public async Task<bool> ValidatePassword(User user, string password) {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetUserRoles(User user) {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<User> RegisterUser(UserCreateDto userCreateDto) {

        User user = new User() {
            Email = userCreateDto.Email,
            LastName = userCreateDto.LastName,
            Name = userCreateDto.Name,
            UserName = userCreateDto.UserName,
            PhoneNumber = userCreateDto.PhoneNumber,
            NormalizedEmail = userCreateDto.Email.ToUpper(),
            NormalizedUserName = userCreateDto.UserName.ToUpper(),
        };

        var result = await _userManager.CreateAsync(user, userCreateDto.Password);
        if (!result.Succeeded) {
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
        }

        return user;
    }

    public async Task EnsureDefaultRoles() {
        if (!await _roleManager.RoleExistsAsync("Admin")) {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await _roleManager.RoleExistsAsync("Registered")) {
            await _roleManager.CreateAsync(new IdentityRole("Registered"));
        }
    }

    public async Task AddUserToRole(User user, string roleName) {
        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded) {
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            throw new InfrastructureException($"Could not add user to role '{roleName}'. {errors}");
        }
    }

    public async Task UpdateUser(User user) {
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) {
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            throw new InfrastructureException($"Could not update user. {errors}");
        }
    }
}