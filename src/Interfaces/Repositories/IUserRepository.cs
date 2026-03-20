using ApiMovies.Models.Entities;

namespace ApiMovies.Interfaces.Repositories;

// Consultas y actualizaciones sobre la entidad User (tabla Identity personalizada).
public interface IUserRepository
{
    // Listado en memoria filtrado por estado activo.
    ICollection<User> GetUsers(bool isActive = true);

    Task<User?> GetUser(string userId);

    // Login: busca por UserName o Email normalizados y estado activo.
    Task<User?> GetByUserNameOrEmail(string? userName, string? email, bool isActive = true);

    // Búsqueda por substring en nombre o email (minúsculas).
    ICollection<User> SearchUsers(string search, bool isActive = true   );

    // Existe id y está activo.
    Task<bool> UserExists(string userId);

    Task<bool> UserNameExists(string userName);

    Task<bool> EmailExists(string email);

    // Actualiza perfil parcial (imagen solo si viene valor no vacío).
    Task<bool> UpdateUser(string userId, User user);

    Task<bool> ActivateUser(string userId);

    // Baja lógica: desactiva y bloquea el usuario.
    Task<bool> DisableUser(string userId);
}
