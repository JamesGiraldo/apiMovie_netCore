using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

public interface IAuthRepository {

    Task<UserLoginResponseDto> LoginUser(UserLoginDto userLoginDto);

    Task<UserInfoDto> RegisterUser(UserCreateDto userCreateDto);

}
