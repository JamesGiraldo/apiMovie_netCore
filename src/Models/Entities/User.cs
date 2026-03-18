using Microsoft.AspNetCore.Identity;

namespace ApiMovies.Models.Entities;

public class User : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Image { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
