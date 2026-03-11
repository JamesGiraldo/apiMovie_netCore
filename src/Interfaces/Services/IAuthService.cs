using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface IAuthService {

    Task<UserLoginResponseDto> LoginUser(UserLoginDto userLoginDto);

    Task<UserInfoDto> RegisterUser(UserCreateDto userCreateDto);

}