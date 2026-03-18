using ApiMovies.Models.Dtos;
using ApiMovies.Common.Pagination;
namespace ApiMovies.Interfaces.Services;

public interface IUserService {

    Task<PagedResult<UserDto>> GetUsers(string? search = null, PaginationQuery? paginationQuery = null);
    Task<UserDto> GetUser(string userId);
    Task<UserDto> UpdateUser(string userId, UserUpdateDto userDto);
    Task<UserDto> ActivateUser(string userId);
    Task<UserDto> DeleteUser(string userId);

}
