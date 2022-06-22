using Microsoft.AspNetCore.Mvc;

namespace MyFinance.Presentation.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected ActionResult SuccessResponse(object? result = null)
        {
            if (result is null)
                return NoContent();

            return Ok(result);
        }
    }
}
