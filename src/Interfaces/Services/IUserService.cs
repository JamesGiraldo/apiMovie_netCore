using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface IUserService {

    ICollection<UserDto> GetUsers(bool isActive = true, string? search = null);
    UserDto GetUser(int userId, bool isActive = true);
    Task<UserDto> UpdateUser(int userId, UserDto userDto);
    Task<UserDto> DeleteUser(int userId);

}
