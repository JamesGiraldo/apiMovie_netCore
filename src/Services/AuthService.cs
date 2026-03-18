using ApiMovies.Interfaces.Services;
using ApiMovies.Interfaces.Repositories;
using ApiMovies.Models.Dtos;
using ApiMovies.Common.Exceptions;

namespace ApiMovies.Services;

public class AuthService : IAuthService {

    private readonly IAuthRepository _authRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUserResponseFactory _userResponseFactory;

    public AuthService(
        IAuthRepository authRepository,
        IUserRepository userRepository,
        IFileStorageService fileStorageService,
        IUserResponseFactory userResponseFactory
    ) {
        _authRepository = authRepository;
        _userRepository = userRepository;
        _fileStorageService = fileStorageService;
        _userResponseFactory = userResponseFactory;
    }

    public async Task<UserResponseDto> LoginUser(UserLoginDto userLoginDto) {
        try {
            ValidateLoginUser(userLoginDto);
            var user = await _authRepository.GetUserForLogin(userLoginDto.UserName, userLoginDto.Email);
            const string InvalidCredentialsMessage = "Invalid credentials.";

            if (user is null) {
                throw new UnauthorizedException(InvalidCredentialsMessage);
            }

            var isValid = await _authRepository.ValidatePassword(user, userLoginDto.Password);
            if (!isValid) {
                throw new UnauthorizedException(InvalidCredentialsMessage);
            }

            var roles = await _authRepository.GetUserRoles(user);
            var response = _userResponseFactory.Create(user, roles);

            return response;
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            throw new InfrastructureException(
                "An unexpected error occurred while logging in.",
                ex
            );
        }
    }

    public async Task<UserResponseDto> RegisterUser(UserCreateDto userCreateDto) {
        try {
            await ValidateRegisterUser(userCreateDto);
            var user = await _authRepository.RegisterUser(userCreateDto);
            await _authRepository.EnsureDefaultRoles();
            await _authRepository.AddUserToRole(user, "Admin");

            if (userCreateDto.Image is not null) {
                var uploadResult = await _fileStorageService.UploadImageAsync(userCreateDto.Image, "users", user.Id);
                user.Image = uploadResult.Url;

                try {
                    await _authRepository.UpdateUser(user);
                } catch {
                    await SafeDeleteImageAsync(uploadResult.Url);
                    throw;
                }
            }

            var roles = await _authRepository.GetUserRoles(user);
            var response = _userResponseFactory.Create(user, roles);

            return response;
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            throw new InfrastructureException(
                "An unexpected error occurred while registering the user.",
                ex
            );
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

    private async Task SafeDeleteImageAsync(string? fileUrl) {
        if (string.IsNullOrWhiteSpace(fileUrl)) {
            return;
        }

        try {
            await _fileStorageService.DeleteByUrlAsync(fileUrl);
        } catch {
            // Intentionally swallow exceptions on compensation cleanup.
        }
    }
}