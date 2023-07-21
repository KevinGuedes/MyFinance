using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Presentation.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseController
{
    [Route("/error")]
    public IActionResult BuildErrorResponse()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var internalServerError = new InternalServerError();
        internalServerError.CausedBy(exception!.Error);

        return BuildInternalServerErrorResponse(internalServerError);
    }
}
