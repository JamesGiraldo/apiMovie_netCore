using Microsoft.EntityFrameworkCore;
using ApiMovies.Models;

namespace ApiMovies.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    // Aqui pasar todas las entidades (Modelos)
    public DbSet<Models.Category> Categories { get; set; }
}