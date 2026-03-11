using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

public interface IAuthRepository {

    Task<UserResponseDto> LoginUser(UserLoginDto userLoginDto);

    Task<UserResponseDto> RegisterUser(UserCreateDto userCreateDto);

}
