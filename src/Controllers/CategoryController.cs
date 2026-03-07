using ApiMovies.Models.Dtos;
using ApiMovies.Repositories.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _ctRepo;
    private readonly IMapper _mapper;

    public CategoryController(
        ICategoryRepository ctRepo,
        IMapper mapper
    ) {
        _ctRepo = ctRepo;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCategories() {
        var categories = _ctRepo.GetCategories();
        var categoriesDto = new List<CategoryDto>();

        foreach (var category in categories) {
            categoriesDto.Add(_mapper.Map<CategoryDto>(category));
        }

        return Ok(categoriesDto);
    }
}