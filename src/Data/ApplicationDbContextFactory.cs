using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApiMovies.Data;

// Fábrica en tiempo de diseño para EF Core: permite ejecutar migraciones y herramientas (dotnet ef)
// sin levantar la aplicación, cargando la cadena de conexión desde configuración local.
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    // Crea un ApplicationDbContext usando appsettings, user secrets y variables de entorno.
    // Parámetro args: Argumentos de la CLI de EF (normalmente no se usan aquí).
    // Retorna: Contexto configurado con Npgsql y la cadena ConexionSql.
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets<ApplicationDbContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("ConexionSql")
            ?? throw new InvalidOperationException("Connection string 'ConexionSql' is missing in configuration.");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
