using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

public interface IAuthRepository {

    Task<User?> GetUserForLogin(string? userName, string? email);

    Task<bool> ValidatePassword(User user, string password);

    Task<IList<string>> GetUserRoles(User user);

    Task<User> RegisterUser(UserCreateDto userCreateDto);

    Task EnsureDefaultRoles();

    Task AddUserToRole(User user, string roleName);

    Task UpdateUser(User user);

}
