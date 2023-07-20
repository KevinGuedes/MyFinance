using FluentResults;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.Common.Errors;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[Produces("application/json")]
[SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(InternalServerErrorResponse))]
public abstract class BaseController : ControllerBase
{
    protected IActionResult ProcessResult<TResponse>(Result<TResponse> result)
    {
        if (result.IsSuccess) return Ok(result.Value);
        return HandleFailureResult(result.Errors);
    }

    protected IActionResult ProcessResult(Result result)
    {
        if (result.IsSuccess) return NoContent();
        return HandleFailureResult(result.Errors);
    }

    private IActionResult HandleFailureResult(List<IError> errors)
    {
        var invalidRequestData = errors.OfType<InvalidRequest>().FirstOrDefault();

        return invalidRequestData is null ?
            BuildInternalServerErrorResponse(errors) :
            BuildBadRequestResponse(invalidRequestData);
    }

    private IActionResult BuildBadRequestResponse(InvalidRequest invalidRequestData)
    {
        var validationErrors = invalidRequestData.ValidationErrors;
        var badRequestResponse = new BadRequestResponse("One or more validation errors occurred.", validationErrors);
        return BadRequest(badRequestResponse);
    }
        

    private IActionResult BuildInternalServerErrorResponse(List<IError> errors)
    {
        var errorMessages = errors.Select(error => error.Message).ToList();
        var internalServerErrorResponse = new InternalServerErrorResponse("MyFinance API went rogue! Sorry.", errorMessages);
        return StatusCode(StatusCodes.Status500InternalServerError, internalServerErrorResponse);
    }
}
