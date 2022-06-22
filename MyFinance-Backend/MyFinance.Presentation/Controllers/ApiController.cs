using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        protected ActionResult FailureResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values
                .SelectMany(error => error.Errors)
                .Select(error => error.ErrorMessage);

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", errors.ToArray() }
            }));
        }
    }
}
