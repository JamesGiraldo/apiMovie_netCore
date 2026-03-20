using ApiMovies.Models.Dtos;
using ApiMovies.Common.Pagination;
namespace ApiMovies.Interfaces.Services;

// Administración de usuarios activos: listado, detalle, actualización con imagen y baja lógica (desactivación).
public interface IUserService {

    // Usuarios activos con búsqueda opcional por nombre o email.
    Task<PagedResult<UserDto>> GetUsers(string? search = null, PaginationQuery? paginationQuery = null);

    // Perfil con roles e URLs de imagen.
    Task<UserDto> GetUser(string userId);

    // Actualiza datos básicos y opcionalmente la foto; valida unicidad de email/username.
    Task<UserDto> UpdateUser(string userId, UserUpdateDto userDto);

    // Reactiva cuenta y limpia bloqueos de Identity.
    Task<UserDto> ActivateUser(string userId);

    // Desactiva usuario y aplica bloqueo indefinido en Identity.
    Task<UserDto> DeleteUser(string userId);

}
