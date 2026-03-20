using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

// Contrato de autenticación: inicio de sesión y registro devolviendo token JWT y datos del usuario.
// Las implementaciones validan credenciales, unicidad y propagan ApiMovies.Common.Exceptions.AppException según el caso.
public interface IAuthService {

    // Autentica por nombre de usuario o email y contraseña; lanza si las credenciales son inválidas.
    // Parámetro userLoginDto: Credenciales del cliente.
    // Retorna: Información del usuario, roles y token.
    Task<UserResponseDto> LoginUser(UserLoginDto userLoginDto);

    // Crea la cuenta en Identity, asigna rol por defecto y opcionalmente sube imagen de perfil.
    // Parámetro userCreateDto: Datos de registro (puede incluir archivo multipart).
    // Retorna: Misma forma que el login: usuario y JWT.
    Task<UserResponseDto> RegisterUser(UserCreateDto userCreateDto);

}
