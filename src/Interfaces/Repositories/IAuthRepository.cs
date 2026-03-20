using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

// Persistencia y validación de credenciales para flujos de autenticación (Identity).
public interface IAuthRepository {

    // Busca usuario activo por nombre normalizado o email normalizado.
    Task<User?> GetUserForLogin(string? userName, string? email);

    // Verifica la contraseña con el hash almacenado por UserManager.
    Task<bool> ValidatePassword(User user, string password);

    // Crea el usuario en Identity; si falla, la implementación agrega el detalle de errores.
    Task<User> RegisterUser(UserCreateDto userCreateDto);

    // Persiste cambios al usuario (p. ej. imagen tras subida a S3).
    Task UpdateUser(User user);

}
