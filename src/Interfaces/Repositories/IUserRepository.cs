using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

public interface IUserRepository
{
    ICollection<User> GetUsers(bool isActive = true);

    Task<User?> GetUser(string userId);

    Task<User?> GetByUserNameOrEmail(string? userName, string? email, bool isActive = true);

    ICollection<User> SearchUsers(string search, bool isActive = true   );

    Task<bool> UserExists(string userId);

    Task<bool> UserNameExists(string userName);

    Task<bool> EmailExists(string email);

    Task<bool> UpdateUser(string userId, User user);

    Task<bool> ActivateUser(string userId);

    Task<bool> DisableUser(string userId);

    Task<bool> Save();
}
