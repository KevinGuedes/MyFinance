using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiService;

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
            var message = exception is null ? "Unexpected server behavior" : exception.Error.Message;
            var apiErrorResponse = new InternalServerErrorResponse("MyFinance API went rogue! Sorry.", message);

            return StatusCode(StatusCodes.Status500InternalServerError, apiErrorResponse);
        }
    }
}
