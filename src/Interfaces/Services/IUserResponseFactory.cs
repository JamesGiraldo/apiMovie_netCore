using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Services;

public interface IUserResponseFactory {
    UserResponseDto Create(User user, IList<string> roles);
}
