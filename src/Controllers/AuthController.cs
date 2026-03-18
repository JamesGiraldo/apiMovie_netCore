using ApiMovies.Common.Responses;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ApiMovies.Controllers;

[Route("api/v{version:apiVersion}/auth")]
[ApiController]
[ApiVersionNeutral]
public class AuthController : ControllerBase {

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(201, Type = typeof(UserResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterUser([FromForm] UserCreateDto userCreateDto) {
        var user = await _authService.RegisterUser(userCreateDto);
        return this.ApiSuccess(
            title: "User registered successfully.",
            statusCode: StatusCodes.Status201Created,
            data: user
        );
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(UserResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLoginDto) {
        var user = await _authService.LoginUser(userLoginDto);
        return this.ApiSuccess(
            title: "User logged in successfully.",
            statusCode: StatusCodes.Status200OK,
            data: user
        );
    }
}