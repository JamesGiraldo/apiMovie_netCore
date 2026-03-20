using ApiMovies.Interfaces.Repositories;
using ApiMovies.Interfaces.Services;
using ApiMovies.Repositories;
using ApiMovies.Services;

namespace ApiMovies.Data;

// Registro central de repositorios (acceso a datos) y servicios de aplicación (casos de uso).
public static class DependencyInjection
{
    // Cada repositorio vive en scope HTTP: nueva instancia por petición (adecuado con DbContext scoped).
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    // Servicios que orquestan validación, mapeos y llamadas a repositorios / storage.
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserResponseFactory, UserResponseFactory>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
