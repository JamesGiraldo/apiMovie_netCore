using ApiMovies.Repositories.IRepository;
using ApiMovies.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ApiMovies.Services.IService;
using ApiMovies.Services;

namespace ApiMovies.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // add repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // add services
        services.AddScoped<IMovieService, MovieService>();
        return services;
    }
}