using ApiMovies.Common.Constants;
using ApiMovies.Common.Responses;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers.V1;

[Route("api/v{version:apiVersion}/roles")]
[ApiController]
[Authorize(Roles = RoleNames.Admin)]
public class RoleController : ControllerBase {
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService) {
        _roleService = roleService;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyCollection<RoleDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoles() {
        var roles = await _roleService.GetRoles();
        return this.ApiSuccess(
            title: "Roles retrieved successfully.",
            statusCode: StatusCodes.Status200OK,
            data: roles
        );
    }

    [HttpPost("users/{userId}")]
    [ProducesResponseType(200, Type = typeof(UserRolesDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignRoleToUser(string userId, [FromBody] AssignUserRoleDto assignUserRoleDto) {
        var userWithRoles = await _roleService.AssignRoleToUser(userId, assignUserRoleDto);
        return this.ApiSuccess(
            title: "Role assigned successfully.",
            statusCode: StatusCodes.Status200OK,
            data: userWithRoles
        );
    }
}
