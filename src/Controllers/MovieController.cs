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
        return this.FromServiceResult(
            _mService.GetMovies(search),
            successTitle: string.IsNullOrWhiteSpace(search)
                ? "Movies retrieved successfully."
                : "Movies filtered by search successfully.",
            successStatus: StatusCodes.Status200OK
        );
    }

    [HttpGet("{movieId:int}", Name = "GetMovie")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetMovie(int movieId) {
        return this.FromServiceResult(
            _mService.GetMovie(movieId),
            successTitle: "Movie retrieved successfully.",
            successStatus: StatusCodes.Status200OK
        );
    }

    [HttpGet("category/{categoryId:int}", Name = "GetMoviesByCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetMoviesByCategory(int categoryId, [FromQuery] string? search) {
        return this.FromServiceResult(
            _mService.GetMoviesByCategory(categoryId, search),
            successTitle: string.IsNullOrWhiteSpace(search)
                ? "Movies by category retrieved successfully."
                : "Movies by category filtered by search successfully.",
            successStatus: StatusCodes.Status200OK
        );
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(MovieCreateDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateMovie([FromBody] MovieCreateDto movieDto) {
        return this.FromServiceResult(
            _mService.CreateMovie(movieDto),
            successTitle: "Movie created successfully.",
            successStatus: StatusCodes.Status201Created
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
        return this.FromServiceResult(
            _mService.UpdateMovie(movieId, movieDto),
            successTitle: "Movie updated successfully.",
            successStatus: StatusCodes.Status200OK
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
        return this.FromServiceResult(
            _mService.ReplaceMovie(movieId, movieDto),
            successTitle: "Movie replaced successfully.",
            successStatus: StatusCodes.Status200OK
        );
    }

    [HttpDelete("{movieId:int}", Name = "DeleteMovie")]
    [ProducesResponseType(200, Type = typeof(MovieDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteMovie(int movieId) {
        return this.FromServiceResult(
            _mService.DeleteMovie(movieId),
            successTitle: "Movie deleted successfully.",
            successStatus: StatusCodes.Status200OK
        );
    }

    private IActionResult FromServiceResult<T>(
        ServiceResult<T> result,
        string successTitle,
        int successStatus = StatusCodes.Status200OK
    ) {
        if (result.Succeeded)
        {
            return this.ApiSuccess(
                title: successTitle,
                statusCode: successStatus,
                data: result.Value,
                detail: result.Detail
            );
        }

        return this.ApiFailure(
            title: result.Title ?? "Request failed.",
            statusCode: MapStatusCode(result.ErrorCode),
            detail: result.Detail
        );
    }

    private static int MapStatusCode(string? errorCode) => errorCode switch {
        "InvalidPayload" or "InvalidName" or "InvalidId" or "RouteBodyIdMismatch" => StatusCodes.Status400BadRequest,
        "NotFound" => StatusCodes.Status404NotFound,
        "DuplicateName" => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };
}