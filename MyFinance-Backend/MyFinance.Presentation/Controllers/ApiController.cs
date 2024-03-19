using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.Errors;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/problem+json")]
[Consumes("application/json")]
[SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(ProblemDetails))]
public abstract class ApiController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator _mediator = mediator;

    protected IActionResult ProcessResult<TResponse>(Result<TResponse> result, bool hasEntityBeenCreated = false)
    {
        if (result.IsFailed)
            return HandleFailureResult(result.Errors);

        return hasEntityBeenCreated ?
            StatusCode(StatusCodes.Status201Created, result.Value) :
            Ok(result.Value);
    }

    protected IActionResult ProcessResult(Result result)
        => result.IsSuccess ? NoContent() : HandleFailureResult(result.Errors);

    protected IActionResult HandleFailureResult(List<IError> errors)
    {
        return errors.FirstOrDefault() switch
        {
            InvalidRequestError invalidRequestError
                => BuildValidationProblemResult(invalidRequestError),
            EntityNotFoundError entityNotFoundError
                => BuildProblemResult(StatusCodes.Status404NotFound, entityNotFoundError),
            UnprocessableEntityError unprocessableEntityError
                => BuildProblemResult(StatusCodes.Status422UnprocessableEntity, unprocessableEntityError),
            UnauthorizedError unauthorizedError
                => BuildProblemResult(StatusCodes.Status401Unauthorized, unauthorizedError),
            ConflictError conflictError
                => BuildProblemResult(StatusCodes.Status409Conflict, conflictError),
            _
                => BuildProblemResult(StatusCodes.Status500InternalServerError)
        };
    }

    private ObjectResult BuildProblemResult(int statusCode, IError error)
        => Problem(statusCode: statusCode, detail: error!.Message);

    private ObjectResult BuildProblemResult(int statusCode)
       => Problem(statusCode: statusCode, detail: "MyFinance API went rogue! Sorry!");

    private ActionResult BuildValidationProblemResult(InvalidRequestError invalidRequestError)
    {
        var validationErrors = invalidRequestError.ValidationErrors;
        var validationProblemDetails = new ValidationProblemDetails(validationErrors);
        return ValidationProblem(validationProblemDetails);
    }
}