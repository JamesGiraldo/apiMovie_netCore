using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface ITokenService {
    string GenerateToken(UserInfoDto userInfoDto);
}
