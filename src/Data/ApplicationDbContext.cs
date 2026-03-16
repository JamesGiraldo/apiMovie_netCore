using Microsoft.EntityFrameworkCore;
using ApiMovies.Models.Entities;

namespace ApiMovies.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Aqui pasar todas las entidades (Entities)
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Movie> Movie => Set<Movie>();
    public DbSet<User> User => Set<User>();
}