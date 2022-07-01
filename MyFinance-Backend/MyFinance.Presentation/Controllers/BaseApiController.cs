using FluentResults;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Generics.ApiService;
using MyFinance.Application.Generics.Errors;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers
{
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(ApiErrorResponse))]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult ProcessResult<TResponse>(Result<TResponse> result)
        {
            if (result.IsSuccess) return Ok(result.Value);
            return BuildApiErrorResponse(result.Errors);
        }

        protected IActionResult ProcessResult(Result result)
        {
            if (result.IsSuccess) return NoContent();
            return BuildApiErrorResponse(result.Errors);
        }

        private IActionResult BuildApiErrorResponse(List<IError> errors)
        {
            var invalidRequestDataError = errors
                .Where(error => error is InvalidRequestData)
                .FirstOrDefault() as InvalidRequestData;

            return invalidRequestDataError is not null ?
                BuildBadRequestResponse(invalidRequestDataError.ValidationErrors) :
                BuildInternalServerErrorResponse(errors);
        }

        private IActionResult BuildBadRequestResponse(Dictionary<string, string[]> validationErrors)
        {
            var status = StatusCodes.Status400BadRequest;
            var title = "One or more validation errors occurred.";
            return BadRequest(new ApiInvalidDataResponse(status, title, validationErrors));
        }

        private IActionResult BuildInternalServerErrorResponse(List<IError> errors)
        {
            var status = StatusCodes.Status500InternalServerError;
            var title = "MyFinance API went rogue! Sorry.";
            var errorMessages = errors.Select(error => error.Message).ToList();
            return StatusCode(status, new ApiErrorResponse(status, title, errorMessages));
        }
    }
}
