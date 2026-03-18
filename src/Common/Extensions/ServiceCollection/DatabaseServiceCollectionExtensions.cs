using ApiMovies.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Common.Extensions;

public static class DatabaseServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ConexionSql")
            ?? throw new InvalidOperationException("Connection string 'ConexionSql' is missing in configuration.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
