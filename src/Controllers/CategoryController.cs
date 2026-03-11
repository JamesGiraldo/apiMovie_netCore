using ApiMovies.Common.Responses;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _ctService;

    public CategoryController(ICategoryService ctService) {
        _ctService = ctService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCategories([FromQuery] string? search) {
        return this.FromServiceResult(
            _ctService.GetCategories(search),
            successTitle: string.IsNullOrWhiteSpace(search)
                ? "Categories retrieved successfully."
                : "Categories filtered by search successfully.",
            successStatus: StatusCodes.Status200OK
        );
    }

    [HttpGet("{categoryId:int}", Name = "GetCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCategory(int categoryId) {
        return this.FromServiceResult(
            _ctService.GetCategory(categoryId),
            successTitle: "Category retrieved successfully.",
            successStatus: StatusCodes.Status200OK
        );
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(CategoryCreateDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryCreateDto) {
        return this.FromServiceResult(
            _ctService.CreateCategory(categoryCreateDto),
            successTitle: "Category created successfully.",
            successStatus: StatusCodes.Status201Created
        );
    }

    [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto dto) {
        return this.FromServiceResult(
            _ctService.UpdateCategory(categoryId, dto),
            successTitle: "Category updated successfully.",
            successStatus: StatusCodes.Status200OK
        );
    }

    [HttpPut("{categoryId:int}", Name = "ReplaceCategory")]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ReplaceCategory(int categoryId, [FromBody] CategoryDto dto) {
        return this.FromServiceResult(
            _ctService.ReplaceCategory(categoryId, dto),
            successTitle: "Category replaced successfully.",
            successStatus: StatusCodes.Status200OK
        );
    }

    [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteCategory(int categoryId) {
        return this.FromServiceResult(
            _ctService.DeleteCategory(categoryId),
            successTitle: "Category deleted successfully.",
            successStatus: StatusCodes.Status200OK
        );
    }
}