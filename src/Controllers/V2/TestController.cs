using ApiMovies.Common.Responses;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers.V2;

[Route("api/v{version:apiVersion}/test")]
[ApiController]
[ApiVersion("2.0")]
public class TestController : ControllerBase
{
    [HttpGet]
    // [Obsolete("This method is obsolete. Use GetTest2 instead.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public IActionResult GetTest() {
        return this.ApiSuccess(
            title: "Test retrieved successfully.",
            statusCode: StatusCodes.Status200OK,
            data: "Hello World"
        );
    }
}