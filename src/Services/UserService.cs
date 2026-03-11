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

    public UserDto GetUser(int userId, bool isActive = true) {
        try {
            var user = _userRepository.GetUser(userId, isActive);
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
        try {
        if (!await _userRepository.UserExists(userId)) throw new NotFoundException("User not found.");
            var user = await _userRepository.UpdateUser(userId, _mapper.Map<User>(userDto));
            var userResponse = _mapper.Map<UserDto>(user);

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
            if (!await _userRepository.UserExists(userId)) throw new NotFoundException("User not found.");
            var user = await _userRepository.DisableUser(userId);
            if (!user) throw new NotFoundException("User not found.");
            var userResponse = _mapper.Map<UserDto>(user);

            return userResponse;
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error deleting user");
            throw new InfrastructureException(
                "An unexpected error occurred while deleting user.",
                ex
            );
            _logger.LogError(ex.Message);
            throw new BadRequestException(ex.Message);
        }
    }
}