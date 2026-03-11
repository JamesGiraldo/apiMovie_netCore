using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface IAuthService {

    Task<UserResponseDto> LoginUser(UserLoginDto userLoginDto);

    Task<UserResponseDto> RegisterUser(UserCreateDto userCreateDto);

}