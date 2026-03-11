using Microsoft.AspNetCore.Mvc;
using ApiMovies.Common.Responses;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;

namespace ApiMovies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase {

    private readonly IUserService _userService;

    public UserController(IUserService userService) {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetUsers([FromQuery] string? search = null) {
        var users = _userService.GetUsers(search: search);
        return this.ApiSuccess(
            title: "Users retrieved successfully.",
            statusCode: StatusCodes.Status200OK,
            data: users
        );
    }

    [HttpGet("{userId:int}", Name = "GetUser")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetUser(int userId) {
        var user = _userService.GetUser(userId);
        return this.ApiSuccess(
            title: "User retrieved successfully.",
            statusCode: StatusCodes.Status200OK,
            data: user
        );
    }

    [HttpPut("{userId:int}", Name = "UpdateUser")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(int userId, UserDto userDto) {
        var user = await _userService.UpdateUser(userId, userDto);
        return this.ApiSuccess(
            title: "User updated successfully.",
            statusCode: StatusCodes.Status200OK,
            data: user
        );
    }

    [HttpDelete("{userId:int}", Name = "DeleteUser")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(int userId) {
        var user = await _userService.DeleteUser(userId);
        return this.ApiSuccess(
            title: "User deleted successfully.",
            statusCode: StatusCodes.Status200OK,
            data: user
        );
    }
}