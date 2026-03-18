using ApiMovies.Data;
using ApiMovies.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace ApiMovies.Common.Extensions;

public static class SecurityServiceCollectionExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddJwtAuthentication(configuration);

        return services;
    }
}
