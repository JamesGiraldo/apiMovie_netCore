using ApiMovies.Interfaces.Services;
using ApiMovies.Interfaces.Repositories;
using ApiMovies.Models.Dtos;
using ApiMovies.Common.Exceptions;

namespace ApiMovies.Services;

public class AuthService : IAuthService {

    private readonly IAuthRepository _authRepository;
    private readonly IUserRepository _userRepository;

    public AuthService(
        IAuthRepository authRepository,
        IUserRepository userRepository
    ) {
        _authRepository = authRepository;
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> LoginUser(UserLoginDto userLoginDto) {
        try {
            ValidateLoginUser(userLoginDto);
            var response = await _authRepository.LoginUser(userLoginDto);

            return response;
        } catch (Exception ex) {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<UserResponseDto> RegisterUser(UserCreateDto userCreateDto) {
        try {
            await ValidateRegisterUser(userCreateDto);
            var response = await _authRepository.RegisterUser(userCreateDto);

            return response;
        } catch (Exception ex) {
            throw new BadRequestException(ex.Message);
        }
    }

    private static void ValidateLoginUser(UserLoginDto userLoginDto) {
        if (string.IsNullOrWhiteSpace(userLoginDto.UserName) && string.IsNullOrWhiteSpace(userLoginDto.Email)) {
            throw new BadRequestException("User name or email is required.");
        }
    }

    private async Task ValidateUserName(string userName) {
        if (await _userRepository.UserNameExists(userName)) {
            throw new ConflictException("User name already exists.");
        }
    }

    private async Task ValidateRegisterUser(UserCreateDto userCreateDto) {
        await ValidateEmail(userCreateDto.Email);
        await ValidateUserName(userCreateDto.UserName);
    }

    private async Task ValidateEmail(string email) {
        if (await _userRepository.EmailExists(email)) {
            throw new ConflictException("Email already exists.");
        }
    }
}