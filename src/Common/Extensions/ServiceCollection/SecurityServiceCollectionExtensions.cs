using ApiMovies.Data;
using ApiMovies.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace ApiMovies.Common.Extensions;

// Identidad de usuario (tablas en EF) y autenticación con JWT Bearer.
public static class SecurityServiceCollectionExtensions
{
    // Configura Identity sobre ApplicationDbContext y el esquema JWT de la API.
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddJwtAuthentication(configuration);

        return services;
    }
}
