using Microsoft.AspNetCore.Identity;

namespace ApiMovies.Models.Entities;

// Usuario de aplicación sobre IdentityUser: perfil extendido, avatar y bandera de baja lógica.
public class User : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // Ruta/URL del avatar; puede resolverse a URLs firmadas vía IFileStorageService.
    public string? Image { get; set; }

    // Si es falso, el usuario no debe autenticarse ni aparecer en listados de activos.
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
