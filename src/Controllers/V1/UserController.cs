using Microsoft.AspNetCore.Mvc;
using ApiMovies.Common.Responses;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ApiMovies.Controllers.V1;

[Route("api/v{version:apiVersion}/user")]
[ApiController]
public class UserController : ControllerBase {

    private readonly IUserService _userService;

    public UserController(IUserService userService) {
        _userService = userService;
    }

    // [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsers([FromQuery] string? search = null) {
        var users = await _userService.GetUsers(search: search);
        return this.ApiSuccess(
            title: "Users retrieved successfully.",
            statusCode: StatusCodes.Status200OK,
            data: users
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{userId}", Name = "GetUser")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUser(string userId) {
        var user = await _userService.GetUser(userId);
        return this.ApiSuccess(
            title: "User retrieved successfully.",
            statusCode: StatusCodes.Status200OK,
            data: user
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{userId}", Name = "UpdateUser")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(string userId, UserDto userDto) {
        var user = await _userService.UpdateUser(userId, userDto);
        return this.ApiSuccess(
            title: "User updated successfully.",
            statusCode: StatusCodes.Status200OK,
            data: user
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{userId}/activate", Name = "ActivateUser")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActivateUser(string userId) {
        var user = await _userService.ActivateUser(userId);
        return this.ApiSuccess(
            title: "User activated successfully.",
            statusCode: StatusCodes.Status200OK,
            data: user
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}", Name = "DeleteUser")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(string userId) {
        var user = await _userService.DeleteUser(userId);
        return this.ApiSuccess(
            title: "User deleted successfully.",
            statusCode: StatusCodes.Status200OK,
            data: user
        );
    }
}