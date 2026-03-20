using ApiMovies.Models.Entities;
using ApiMovies.Models.Dtos;
using AutoMapper;

namespace ApiMovies.MoviesMappers;

// Perfil AutoMapper central: entidades de dominio ↔ DTOs de API y formularios multipart.
public class MoviesMapper : Profile
{
    // Registra todos los mapeos bidireccionales usados por servicios y factorías de respuesta.
    public MoviesMapper()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryCreateDto>().ReverseMap();

        CreateMap<Movie, MovieDto>().ReverseMap();
        CreateMap<Movie, MovieCreateDto>().ReverseMap();

        CreateMap<User, UserCreateDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserInfoDto>().ReverseMap();
        CreateMap<User, UserLoginDto>().ReverseMap();
        CreateMap<User, UserResponseDto>().ReverseMap();
    }

}