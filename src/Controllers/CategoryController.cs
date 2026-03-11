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
        var categories = _ctService.GetCategories(search);
        return this.ApiSuccess(
            title: string.IsNullOrWhiteSpace(search)
                ? "Categories retrieved successfully."
                : "Categories filtered by search successfully.",
            statusCode: StatusCodes.Status200OK,
            data: categories
        );
    }

    [HttpGet("{categoryId:int}", Name = "GetCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCategory(int categoryId) {
        var category = _ctService.GetCategory(categoryId);
        return this.ApiSuccess(
            title: "Category retrieved successfully.",
            statusCode: StatusCodes.Status200OK,
            data: category
        );
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(CategoryCreateDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryCreateDto) {
        var category = _ctService.CreateCategory(categoryCreateDto);
        return this.ApiSuccess(
            title: "Category created successfully.",
            statusCode: StatusCodes.Status201Created,
            data: category
        );
    }

    [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto dto) {
        var category = _ctService.UpdateCategory(categoryId, dto);
        return this.ApiSuccess(
            title: "Category updated successfully.",
            statusCode: StatusCodes.Status200OK,
            data: category
        );
    }

    [HttpPut("{categoryId:int}", Name = "ReplaceCategory")]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ReplaceCategory(int categoryId, [FromBody] CategoryDto dto) {
        var category = _ctService.ReplaceCategory(categoryId, dto);
        return this.ApiSuccess(
            title: "Category replaced successfully.",
            statusCode: StatusCodes.Status200OK,
            data: category
        );
    }

    [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteCategory(int categoryId) {
        var category = _ctService.DeleteCategory(categoryId);
        return this.ApiSuccess(
            title: "Category deleted successfully.",
            statusCode: StatusCodes.Status200OK,
            data: category
        );
    }
}