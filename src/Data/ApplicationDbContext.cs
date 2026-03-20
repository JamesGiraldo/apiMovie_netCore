using Microsoft.EntityFrameworkCore;
using ApiMovies.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApiMovies.Data;

// Contexto EF Core: tablas de Identity (User) más entidades de negocio (películas, categorías).
public class ApplicationDbContext : IdentityDbContext<User>
{
    // Parámetro options: Opciones del contexto inyectadas en tiempo de ejecución (cadena, proveedor, etc.).
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        // La tabla de usuarios de Identity se mapea al nombre "User" (en lugar de AspNetUsers por defecto).
        builder.Entity<User>().ToTable("User");
    }

    // DbSets expuestos para consultas y migraciones; cada uno corresponde a una tabla.
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Movie> Movie => Set<Movie>();
    public DbSet<User> User => Set<User>();
}