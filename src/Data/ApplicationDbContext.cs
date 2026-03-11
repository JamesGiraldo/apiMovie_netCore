using Microsoft.EntityFrameworkCore;
using ApiMovies.Models.Entities;

namespace ApiMovies.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    // Aqui pasar todas las entidades (Entities)
    public DbSet<Category> Category { get; set; }
    public DbSet<Movie> Movie { get; set; }
    public DbSet<User> User { get; set; }
}