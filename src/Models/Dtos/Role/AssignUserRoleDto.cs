using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos;

// Cuerpo para asignar rol a un usuario; el id del rol es el de ASP.NET Identity.
public class AssignUserRoleDto {
    // Id del rol (IdentityRole.Id).
    [Required(ErrorMessage = "Role id is required.")]
    public string RoleId { get; set; } = string.Empty;
}
