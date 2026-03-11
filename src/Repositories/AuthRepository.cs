using ApiMovies.Interfaces.Repositories;
using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;
using Mapster;
using BCrypt.Net;
using ApiMovies.Common.Exceptions;


namespace ApiMovies.Repositories;

public class AuthRepository : IAuthRepository {

    private readonly IUserRepository _userRepository;

    public AuthRepository(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task<UserLoginResponseDto> LoginUser(UserLoginDto userLoginDto) {
        var user = await _userRepository.GetByUserNameOrEmail(
            userLoginDto.UserName,
            userLoginDto.Email
        );

        if (user is null) throw new NotFoundException("User not found or invalid user name or email.");

        if (!verifyPassword(userLoginDto.Password, user.Password)) {
            throw new BadRequestException("Invalid password.");
        }

        return new UserLoginResponseDto {
            Token = string.Empty,
            User = user.Adapt<UserInfoDto>(),
            Expiration = DateTime.UtcNow
        };
    }

    public async Task<UserInfoDto> RegisterUser(UserCreateDto userCreateDto) {

        var passwordEncrypted = getPasswordEncrypted(userCreateDto.Password);

        var user = new User {
            Email = userCreateDto.Email,
            IsActive = true,
            Name = userCreateDto.Name,
            Password = passwordEncrypted,
            Role = userCreateDto.Role,
            UserName = userCreateDto.UserName,
        };

        if (!await _userRepository.CreateUser(user)) return null!;

        return await Task.FromResult(user.Adapt<UserInfoDto>());
    }

    private string getPasswordEncrypted(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool verifyPassword(string password, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}