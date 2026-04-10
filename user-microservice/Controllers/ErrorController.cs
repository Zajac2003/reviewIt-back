using Microsoft.AspNetCore.Mvc;

namespace user_microservice.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError()
        {
            return Problem(
                detail: "Unexpected error occurred.",
                title: "Server Error",
                statusCode: 500
            );
        }
    }
}
