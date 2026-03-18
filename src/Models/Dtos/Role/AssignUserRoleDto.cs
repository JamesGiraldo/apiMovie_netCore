using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos;

public class AssignUserRoleDto {
    [Required(ErrorMessage = "Role id is required.")]
    public string RoleId { get; set; } = string.Empty;
}
