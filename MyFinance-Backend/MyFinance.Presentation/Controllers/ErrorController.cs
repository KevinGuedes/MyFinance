using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Generics.ApiService;

namespace MyFinance.Presentation.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult BuildErrorResponse()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var message = exception is not null ? exception.Error.Message : "Unexpected server behavior";
            var apiErrorResponse = new ErrorResponse("MyFinance API went rogue! Sorry.", message);

            return StatusCode(StatusCodes.Status500InternalServerError, apiErrorResponse);
        }
    }
}
