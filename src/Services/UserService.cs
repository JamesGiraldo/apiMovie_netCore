using ApiMovies.Interfaces.Repositories;
using ApiMovies.Interfaces.Services;
using ApiMovies.Common.Exceptions;
using ApiMovies.Models.Entities;
using ApiMovies.Models.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ApiMovies.Services;

public class UserService : IUserService {

    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UserService> logger
    ) {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public ICollection<UserDto> GetUsers(bool isActive = true, string? search = null) {
        try {
            var users = string.IsNullOrWhiteSpace(search)
                ? _userRepository.GetUsers(isActive)
                : SearchUsers(search, isActive);
            if (users.Count == 0) {
                var detail = !string.IsNullOrWhiteSpace(search)
                    ? "No users were found with the provided search term."
                    : isActive
                        ? "No active users were found."
                        : "No users were found.";
                throw new NotFoundException(detail);
            }
            var usersResponse = _mapper.Map<ICollection<UserDto>>(users);

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

    public async Task<UserDto> GetUser(int userId, bool isActive = true) {
        try {
            var user = await _userRepository.GetUser(userId, isActive);
            if (user == null) throw new NotFoundException("User not found.");

            var userResponse = _mapper.Map<UserDto>(user);

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

    public async Task<UserDto> UpdateUser(int userId, UserDto userDto) {
        await ValidateUpdateRequest(userId, userDto);
        try {
            var userToUpdate = _mapper.Map<User>(userDto);
            var isUpdated = await _userRepository.UpdateUser(userId, userToUpdate);
            if (!isUpdated) throw new NotFoundException("User not found.");

            var updatedUser = await _userRepository.GetUser(userId);
            if (updatedUser == null) throw new NotFoundException("User not found.");

            var userResponse = _mapper.Map<UserDto>(updatedUser);

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

    public async Task<UserDto> DeleteUser(int userId) {
        try {
            var existingUser = await ValidateDeleteRequest(userId);

            var isDisabled = await _userRepository.DisableUser(userId);
            if (!isDisabled) throw new NotFoundException("User not found.");

            var userResponse = _mapper.Map<UserDto>(existingUser);

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

    private async Task ValidateUpdateRequest(int userId, UserDto userDto) {
        ValidateId(userId, "userId");

        if (userDto is null) throw new BadRequestException("User payload is required.");
        if (userDto.Id <= 0) throw new BadRequestException("User payload id must be greater than 0.");
        if (userDto.Id != userId) {
            throw new BadRequestException($"The userId in route ({userId}) does not match payload id ({userDto.Id}).");
        }

        if (await _userRepository.UserExists(userId, false)) {
            throw new ConflictException("User is inactive and cannot be updated.");
        }

        var currentUser = await _userRepository.GetUser(userId);
        if (currentUser is null) throw new NotFoundException($"User with id {userId} was not found.");

        var isDuplicatedEmail = await _userRepository.EmailExists(userDto.Email)
            && !string.Equals(currentUser.Email, userDto.Email, StringComparison.OrdinalIgnoreCase);

        var isDuplicatedUserName = await _userRepository.UserNameExists(userDto.UserName)
            && !string.Equals(currentUser.UserName, userDto.UserName, StringComparison.OrdinalIgnoreCase);

        if (isDuplicatedEmail) throw new ConflictException($"The email '{userDto.Email}' is already in our records. Please use a different email.");
        if (isDuplicatedUserName) throw new ConflictException($"The user name '{userDto.UserName}' is already in our records. Please use a different user name.");
    }

    private async Task<User> ValidateDeleteRequest(int userId) {
        ValidateId(userId, "userId");

        if (await _userRepository.UserExists(userId, false)) {
            throw new ConflictException("User is already inactive and cannot be deleted again.");
        }

        var existingUser = await _userRepository.GetUser(userId);
        if (existingUser is null) {
            throw new NotFoundException($"User with id {userId} was not found.");
        }

        return existingUser;
    }

    private static void ValidateId(int id, string paramName) {
        if (id <= 0) {
            throw new BadRequestException($"{paramName} must be greater than 0.");
        }
    }
}