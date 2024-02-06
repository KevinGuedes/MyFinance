using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Presentation.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseController
{
    [Route("/error")]
    public IActionResult BuildErrorResponse()
    {
        var internalServerError = new InternalServerError();

        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
        if (exception is not null) internalServerError.CausedBy(exception.Error);

        return BuildInternalServerErrorResponse(internalServerError);
    }
}
