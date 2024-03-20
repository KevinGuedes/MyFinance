using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/problem+json")]
[Consumes("application/json")]
[SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(ProblemResponse))]
public abstract class ApiController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator _mediator = mediator;

    protected IActionResult ProcessResult<TResponse>(Result<TResponse> result, bool hasEntityBeenCreated = false)
    {
        if (result.IsFailed)
            return HandleFailureResult(result.Errors);

        return hasEntityBeenCreated ? StatusCode(StatusCodes.Status201Created, result.Value) : Ok(result.Value);
    }

    protected IActionResult ProcessResult(Result result)
        => result.IsSuccess ? NoContent() : HandleFailureResult(result.Errors);

    protected IActionResult HandleFailureResult(IEnumerable<IError> errors)
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
    private ActionResult BuildValidationProblemResult(InvalidRequestError invalidRequestError)
    {
        var modelStateDictionary = new ModelStateDictionary();

        invalidRequestError
            .ValidationErrors
            .ToList()
            .ForEach(error => modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage));

        return ValidationProblem(instance: HttpContext.Request.Path, modelStateDictionary: modelStateDictionary);
    }

    private ObjectResult BuildProblemResult(int statusCode, IError error)
        => Problem(statusCode: statusCode, detail: error.Message, instance: HttpContext.Request.Path);

    private ObjectResult BuildProblemResult(int statusCode)
        => Problem(statusCode: statusCode, detail: "MyFinance API went rogue! Sorry!", instance: HttpContext.Request.Path);
}