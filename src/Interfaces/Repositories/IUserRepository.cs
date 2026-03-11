using ApiMovies.Models.Entities;
using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Repositories;

public interface IUserRepository
{
    ICollection<User> GetUsers(bool isActive = true);

    Task<User?> GetUser(int userId, bool isActive = true);

    Task<User?> GetByUserNameOrEmail(string? userName, string? email, bool isActive = true);

    Task<bool> UserExists(int userId, bool isActive = true);

    Task<bool> UserNameExists(string userName, bool isActive = true);

    Task<bool> EmailExists(string email, bool isActive = true);

    Task<bool> CreateUser(User user);

    Task<bool> UpdateUser(User user);

    Task<bool> DisableUser(int userId, bool isActive = true);

    Task<bool> Save();
}
