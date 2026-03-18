using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Repositories;

public interface IAuthRepository {

    Task<UserResponseDto> LoginUser(UserLoginDto userLoginDto);

    Task<UserResponseDto> RegisterUser(UserCreateDto userCreateDto);

}
