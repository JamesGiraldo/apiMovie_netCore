using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface IUserService {

    Task<ICollection<UserDto>> GetUsers(string? search = null);
    Task<UserDto> GetUser(string userId);
    Task<UserDto> UpdateUser(string userId, UserDto userDto);
    Task<UserDto> ActivateUser(string userId);
    Task<UserDto> DeleteUser(string userId);

}
