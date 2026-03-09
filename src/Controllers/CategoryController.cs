using ApiMovies.Models.Dtos;
using ApiMovies.Repositories.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ApiMovies.Models;

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

    [HttpGet("{categoryId:int}", Name = "GetCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCategory(int categoryId) {

        if (categoryId == 0) return BadRequest();

        var category = _ctRepo.GetCategory(categoryId);
        if (category is null) return NotFound();

        var categoryDto = _mapper.Map<CategoryDto>(category);
        return Ok(categoryDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryCreateDto) {
        if (categoryCreateDto is null) return BadRequest(ModelState);

        if (_ctRepo.ExistsCategoryName(categoryCreateDto.Name)) {
            ModelState.AddModelError("", $"Category {categoryCreateDto.Name} already exists");
            return StatusCode(404, ModelState);
        }

        var category = _mapper.Map<Category>(categoryCreateDto);
        if (!_ctRepo.CreateCategory(category)) {
            ModelState.AddModelError("", $"Something went wrong while saving {categoryCreateDto.Name}");
            return StatusCode(500, ModelState);
        }

        return CreatedAtRoute("GetCategory", new { categoryId = category.Id }, category);
    }

    [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto dto) {
        if (dto is null) return BadRequest(ModelState);

        if (categoryId != dto.Id) return BadRequest(ModelState);

        var categoryExists = _ctRepo.GetCategory(categoryId);
        if (categoryExists is null) return NotFound($"Category with id {categoryId} not found");

        if (_ctRepo.ExistsCategoryName(dto.Name)) {
            ModelState.AddModelError("", $"Category {dto.Name} already exists");
            return StatusCode(404, ModelState);
        }

        var category = _mapper.Map<Category>(dto);
        if (!_ctRepo.UpdateCategory(category)) {
            ModelState.AddModelError("", $"Something went wrong while updating {dto.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpPut("{categoryId:int}", Name = "UpdateCategoryPut")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateCategoryPut(int categoryId, [FromBody] CategoryDto dto) {
        if (dto is null) return BadRequest(ModelState);

        if ( categoryId != 0 && categoryId != dto.Id) return BadRequest(ModelState);

        var categoryExists = _ctRepo.GetCategory(categoryId);
        if (categoryExists is null) return NotFound($"Category with id {categoryId} not found");

        if (_ctRepo.ExistsCategoryName(dto.Name)) {
            ModelState.AddModelError("", $"Category {dto.Name} already exists");
            return StatusCode(404, ModelState);
        }

        var category = _mapper.Map<Category>(dto);
        if (!_ctRepo.UpdateCategory(category)) {
            ModelState.AddModelError("", $"Something went wrong while updating {dto.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteCategory(int categoryId) {
        if (categoryId == 0) return BadRequest();

        var category = _ctRepo.GetCategory(categoryId);
        if (category is null) return NotFound($"Category with id {categoryId} not found");

        if (!_ctRepo.DeleteCategory(categoryId)) {
            ModelState.AddModelError("", $"Something went wrong while deleting {category.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

}