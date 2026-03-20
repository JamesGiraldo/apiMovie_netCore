using ApiMovies.Common.Responses;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers.V2;

// Controlador de prueba para verificar versionado de API en v2.
[Route("api/v{version:apiVersion}/test")]
[ApiController]
[ApiVersion("2.0")]
public class TestController : ControllerBase
{
    // Respuesta fija para comprobar enrutamiento y formato ApiMovies.Common.Responses.ApiResponse.
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