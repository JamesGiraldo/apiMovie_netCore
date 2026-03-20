using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

// Emisión de tokens JWT firmados con la clave configurada en ApiSettings:SecretKey.
public interface ITokenService {
    // Genera un JWT con claims de nombre, email y roles concatenados.
    // Parámetro userInfoDto: Datos del usuario ya resueltos para claims.
    // Retorna: Cadena JWT lista para el encabezado Authorization.
    string GenerateToken(UserInfoDto userInfoDto);
}
