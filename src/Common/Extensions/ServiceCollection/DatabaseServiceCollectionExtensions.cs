using ApiMovies.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Common.Extensions;

// Configura Entity Framework Core con PostgreSQL (Npgsql).
public static class DatabaseServiceCollectionExtensions
{
    // Lee la cadena ConexionSql y registra ApplicationDbContext como scoped.
    public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ConexionSql")
            ?? throw new InvalidOperationException("Connection string 'ConexionSql' is missing in configuration.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
