using Microsoft.AspNetCore.Mvc;
using ApiMovies.Common.Responses;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ApiMovies.Controllers.V1;

[Route("api/v{version:apiVersion}/movie")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieService _mService;

    public MovieController(IMovieService mService) {
        _mService = mService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetMovies([FromQuery] string? search) {
        var movies = _mService.GetMovies(search);
        return this.ApiSuccess(
            title: string.IsNullOrWhiteSpace(search)
                ? "Movies retrieved successfully."
                : "Movies filtered by search successfully.",
            statusCode: StatusCodes.Status200OK,
            data: movies
        );
    }

    [AllowAnonymous]
    [HttpGet("{movieId:int}", Name = "GetMovie")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetMovie(int movieId) {
        var movie = _mService.GetMovie(movieId);
        return this.ApiSuccess(
            title: "Movie retrieved successfully.",
            statusCode: StatusCodes.Status200OK,
            data: movie
        );
    }

    [AllowAnonymous]
    [HttpGet("category/{categoryId:int}", Name = "GetMoviesByCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetMoviesByCategory(int categoryId, [FromQuery] string? search) {
        var movies = _mService.GetMoviesByCategory(categoryId, search);
        return this.ApiSuccess(
            title: string.IsNullOrWhiteSpace(search)
                ? "Movies by category retrieved successfully."
                : "Movies by category filtered by search successfully.",
            statusCode: StatusCodes.Status200OK,
            data: movies
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(201, Type = typeof(MovieCreateDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateMovie([FromForm] MovieCreateDto movieDto) {
        var movie = _mService.CreateMovie(movieDto);
        return this.ApiSuccess(
            title: "Movie created successfully.",
            statusCode: StatusCodes.Status201Created,
            data: movie
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{movieId:int}", Name = "UpdateMovie")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(200, Type = typeof(MovieDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateMovie(int movieId, [FromForm] MovieUpdateDto movieDto) {
        var movie = await _mService.UpdateMovie(movieId, movieDto);
        return this.ApiSuccess(
            title: "Movie updated successfully.",
            statusCode: StatusCodes.Status200OK,
            data: movie
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{movieId:int}", Name = "ReplaceMovie")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(200, Type = typeof(MovieDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReplaceMovie(int movieId, [FromForm] MovieUpdateDto movieDto) {
        var movie = await _mService.ReplaceMovie(movieId, movieDto);
        return this.ApiSuccess(
            title: "Movie replaced successfully.",
            statusCode: StatusCodes.Status200OK,
            data: movie
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{movieId:int}", Name = "DeleteMovie")]
    [ProducesResponseType(200, Type = typeof(MovieDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteMovie(int movieId) {
        var movie = _mService.DeleteMovie(movieId);
        return this.ApiSuccess(
            title: "Movie deleted successfully.",
            statusCode: StatusCodes.Status200OK,
            data: movie
        );
    }

}