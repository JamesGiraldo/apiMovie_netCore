using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

public interface IAuthRepository {

    Task<User?> GetUserForLogin(string? userName, string? email);

    Task<bool> ValidatePassword(User user, string password);

    Task<User> RegisterUser(UserCreateDto userCreateDto);

    Task UpdateUser(User user);

}
