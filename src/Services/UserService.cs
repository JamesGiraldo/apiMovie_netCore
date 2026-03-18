using ApiMovies.Interfaces.Repositories;
using ApiMovies.Interfaces.Services;
using ApiMovies.Common.Exceptions;
using ApiMovies.Models.Entities;
using ApiMovies.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ApiMovies.Services;

public class UserService : IUserService {

    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IFileStorageService _fileStorageService;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UserService> logger,
        UserManager<User> userManager,
        IFileStorageService fileStorageService
    ) {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _fileStorageService = fileStorageService;
    }

    public async Task<ICollection<UserDto>> GetUsers(string? search = null) {
        try {
            var users = string.IsNullOrWhiteSpace(search)
                ? _userRepository.GetUsers(isActive: true)
                : SearchUsers(search);
            if (users.Count == 0) {
                var detail = !string.IsNullOrWhiteSpace(search)
                    ? "No users were found with the provided search term."
                    : "No users were found.";
                throw new NotFoundException(detail);
            }
            var usersResponse = new List<UserDto>(users.Count);
            foreach (var user in users) {
                usersResponse.Add(await MapUserWithRolesAsync(user));
            }

            return usersResponse;
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting users");
            throw new InfrastructureException(
                "An unexpected error occurred while retrieving users.",
                ex
            );
        }
    }

    private ICollection<User> SearchUsers(string search, bool isActive = true) {
        return _userRepository.SearchUsers(search, isActive);
    }

    public async Task<UserDto> GetUser(string userId) {
        try {
            var user = await _userRepository.GetUser(userId);
            if (user == null) throw new NotFoundException("User not found.");

            var userResponse = await MapUserWithRolesAsync(user);

            return userResponse;
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting user");
            throw new InfrastructureException(
                "An unexpected error occurred while retrieving user.",
                ex
            );
        }
    }

    public async Task<UserDto> UpdateUser(string userId, UserUpdateDto userDto) {
        await ValidateUpdateRequest(userId, userDto);
        try {
            var currentUser = await _userRepository.GetUser(userId);
            if (currentUser is null) throw new NotFoundException("User not found.");

            var previousImageUrl = currentUser.Image;
            FileUploadResultDto? uploadResult = null;
            if (userDto.Image is not null) {
                uploadResult = await _fileStorageService.UploadImageAsync(userDto.Image, "users", userId);
            }

            var userToUpdate = new User {
                Id = userId,
                Name = userDto.Name.Trim(),
                UserName = userDto.UserName.Trim(),
                Email = userDto.Email.Trim(),
                Image = uploadResult?.Url
            };
            var isUpdated = await _userRepository.UpdateUser(userId, userToUpdate);
            if (!isUpdated) {
                await SafeDeleteAsync(uploadResult?.Url);
                throw new NotFoundException("User not found.");
            }

            if (!string.IsNullOrWhiteSpace(uploadResult?.Url)) {
                await SafeDeleteAsync(previousImageUrl);
            }

            var updatedUser = await _userRepository.GetUser(userId);
            if (updatedUser == null) throw new NotFoundException("User not found.");

            var userResponse = await MapUserWithRolesAsync(updatedUser);

            return userResponse;
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error updating user");
            throw new InfrastructureException(
                "An unexpected error occurred while updating user.",
                ex
            );
        }
    }

    public async Task<UserDto> ActivateUser(string userId) {
        try {
            var existingUser = await ValidateDeleteRequest(userId);

            var isActivated = await _userRepository.ActivateUser(userId);
            if (!isActivated) throw new NotFoundException("User not found.");

            var userResponse = await MapUserWithRolesAsync(existingUser);

            return userResponse;
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error activating user");
            throw new InfrastructureException(
                "An unexpected error occurred while activating user.",
                ex
            );
        }
    }

    public async Task<UserDto> DeleteUser(string userId) {
        try {
            var existingUser = await ValidateDeleteRequest(userId);
            var previousImageUrl = existingUser.Image;

            var isDisabled = await _userRepository.DisableUser(userId);
            if (!isDisabled) throw new NotFoundException("User not found.");

            existingUser.UpdatedAt = DateTime.UtcNow;
            await _userRepository.Save();

            var userResponse = await MapUserWithRolesAsync(existingUser);

            return userResponse;
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error deleting user");
            throw new InfrastructureException(
                "An unexpected error occurred while deleting user.",
                ex
            );
        }
    }

    private async Task ValidateUpdateRequest(string userId, UserUpdateDto userDto) {
        ValidateId(userId, "userId");
        var validatedUserId = userId;

        if (userDto is null) throw new BadRequestException("User payload is required.");
        if (string.IsNullOrEmpty(userDto.Id)) throw new BadRequestException("User payload id is required.");
        if (userDto.Id != validatedUserId) {
            throw new BadRequestException($"The userId in route ({validatedUserId}) does not match payload id ({userDto.Id}).");
        }

        if (!await _userRepository.UserExists(validatedUserId)) {
            throw new ConflictException("User is inactive and cannot be updated.");
        }

        var currentUser = await _userRepository.GetUser(validatedUserId);
        if (currentUser is null) throw new NotFoundException($"User with id {validatedUserId} was not found.");

        var isDuplicatedEmail = await _userRepository.EmailExists(userDto.Email)
            && !string.Equals(currentUser.Email, userDto.Email, StringComparison.OrdinalIgnoreCase);

        var isDuplicatedUserName = await _userRepository.UserNameExists(userDto.UserName)
            && !string.Equals(currentUser.UserName, userDto.UserName, StringComparison.OrdinalIgnoreCase);

        if (isDuplicatedEmail) throw new ConflictException($"The email '{userDto.Email}' is already in our records. Please use a different email.");
        if (isDuplicatedUserName) throw new ConflictException($"The user name '{userDto.UserName}' is already in our records. Please use a different user name.");
    }

    private async Task<User> ValidateDeleteRequest(string userId) {
        ValidateId(userId, "userId");

        var existingUser = await _userRepository.GetUser(userId);
        if (existingUser is null) throw new NotFoundException($"User with id {userId} was not found.");

        return existingUser;
    }

    private async Task<UserDto> MapUserWithRolesAsync(User user) {
        var userResponse = _mapper.Map<UserDto>(user);
        var imageUrls = _fileStorageService.GetFileUrls(user.Image);
        var roles = await _userManager.GetRolesAsync(user);
        userResponse.Image = imageUrls.Url;
        userResponse.ImageUrl = imageUrls.UrlPreview;
        userResponse.Roles = roles.ToArray();
        return userResponse;
    }

    private static void ValidateId(string id, string paramName) {
        if (string.IsNullOrEmpty(id)) {
            throw new BadRequestException($"{paramName} is required.");
        }
    }

    private async Task SafeDeleteAsync(string? fileUrl) {
        if (string.IsNullOrWhiteSpace(fileUrl)) return;

        try {
            await _fileStorageService.DeleteByUrlAsync(fileUrl);
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Could not delete old user image {FileUrl}", fileUrl);
        }
    }
}