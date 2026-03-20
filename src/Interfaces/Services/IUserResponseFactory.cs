using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Services;

// Ensambla la respuesta post login/registro: mapeo a UserInfoDto, URLs de imagen y token.
public interface IUserResponseFactory {
    // Construye UserResponseDto con JWT y expiración alineada al token.
    UserResponseDto Create(User user, IList<string> roles);
}
