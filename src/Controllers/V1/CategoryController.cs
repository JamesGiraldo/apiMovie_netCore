using ApiMovies.Common.Responses;
using ApiMovies.Common.Pagination;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers.V1;

// API REST de categorías: lectura pública con caché en listado; escritura restringida a Admin.
[Route("api/v{version:apiVersion}/category")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // Listado paginado con búsqueda opcional por nombre.
    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(CacheProfileName = "30Seconds")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(200, Type = typeof(PagedResult<CategoryDto>))]
    public async Task<IActionResult> GetCategories(
        [FromQuery] string? search,
        [FromQuery] int pageNumber = PaginationQuery.DefaultPageNumber,
        [FromQuery] int pageSize = PaginationQuery.DefaultPageSize
    )
    {
        var paginationQuery = new PaginationQuery {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var categories = await _categoryService.GetCategories(search, paginationQuery);
        return this.ApiSuccess(
            title: string.IsNullOrWhiteSpace(search)
                ? "Categories retrieved successfully."
                : "Categories filtered by search successfully.",
            statusCode: StatusCodes.Status200OK,
            data: categories
        );
    }

    // Obtiene una categoría por id.
    [AllowAnonymous]
    [HttpGet("{categoryId:int}", Name = "GetCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategory(int categoryId)
    {
        var category = await _categoryService.GetCategory(categoryId);
        return this.ApiSuccess(
            title: "Category retrieved successfully.",
            statusCode: StatusCodes.Status200OK,
            data: category
        );
    }

    // Crea categoría (solo Admin).
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(CategoryCreateDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
    {
        var category = await _categoryService.CreateCategory(categoryCreateDto);
        return this.ApiSuccess(
            title: "Category created successfully.",
            statusCode: StatusCodes.Status201Created,
            data: category
        );
    }

    // Actualización parcial semántica (PATCH).
    [Authorize(Roles = "Admin")]
    [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryDto dto)
    {
        var category = await _categoryService.UpdateCategory(categoryId, dto);
        return this.ApiSuccess(
            title: "Category updated successfully.",
            statusCode: StatusCodes.Status200OK,
            data: category
        );
    }

    // Reemplazo completo (PUT); misma validación que PATCH en capa de servicio.
    [Authorize(Roles = "Admin")]
    [HttpPut("{categoryId:int}", Name = "ReplaceCategory")]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReplaceCategory(int categoryId, [FromBody] CategoryDto dto)
    {
        var category = await _categoryService.ReplaceCategory(categoryId, dto);
        return this.ApiSuccess(
            title: "Category replaced successfully.",
            statusCode: StatusCodes.Status200OK,
            data: category
        );
    }

    // Elimina categoría y devuelve el registro eliminado.
    [Authorize(Roles = "Admin")]
    [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        var category = await _categoryService.DeleteCategory(categoryId);
        return this.ApiSuccess(
            title: "Category deleted successfully.",
            statusCode: StatusCodes.Status200OK,
            data: category
        );
    }
}