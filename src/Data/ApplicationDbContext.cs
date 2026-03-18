using Microsoft.EntityFrameworkCore;
using ApiMovies.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApiMovies.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<User>().ToTable("User");
    }

    // Aqui pasar todas las entidades (Entities)
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Movie> Movie => Set<Movie>();
    public DbSet<User> User => Set<User>();
}