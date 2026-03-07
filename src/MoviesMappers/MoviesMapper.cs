using ApiMovies.Models;
using ApiMovies.Models.Dtos;
using AutoMapper;

namespace ApiMovies.MoviesMappers;

public class MoviesMapper : Profile
{
    public MoviesMapper()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryCreateDto>().ReverseMap();
    }
}