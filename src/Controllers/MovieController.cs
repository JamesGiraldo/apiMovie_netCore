using Microsoft.AspNetCore.Mvc;
using ApiMovies.Common.Responses;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;

namespace ApiMovies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieService _mService;

    public MovieController(IMovieService mService) {
        _mService = mService;
    }

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

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(MovieCreateDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateMovie([FromBody] MovieCreateDto movieDto) {
        var movie = _mService.CreateMovie(movieDto);
        return this.ApiSuccess(
            title: "Movie created successfully.",
            statusCode: StatusCodes.Status201Created,
            data: movie
        );
    }

    [HttpPatch("{movieId:int}", Name = "UpdateMovie")]
    [ProducesResponseType(200, Type = typeof(MovieDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateMovie(int movieId, [FromBody] MovieDto movieDto) {
        var movie = _mService.UpdateMovie(movieId, movieDto);
        return this.ApiSuccess(
            title: "Movie updated successfully.",
            statusCode: StatusCodes.Status200OK,
            data: movie
        );
    }

    [HttpPut("{movieId:int}", Name = "ReplaceMovie")]
    [ProducesResponseType(200, Type = typeof(MovieDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ReplaceMovie(int movieId, [FromBody] MovieDto movieDto) {
        var movie = _mService.ReplaceMovie(movieId, movieDto);
        return this.ApiSuccess(
            title: "Movie replaced successfully.",
            statusCode: StatusCodes.Status200OK,
            data: movie
        );
    }

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