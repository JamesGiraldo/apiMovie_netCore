using ApiMovies.Interfaces.Repositories;
using ApiMovies.Interfaces.Services;
using ApiMovies.Repositories;
using ApiMovies.Services;

namespace ApiMovies.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // add repositories
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // add services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}