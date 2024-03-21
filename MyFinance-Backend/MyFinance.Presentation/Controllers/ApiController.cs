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

    protected ObjectResult HandleFailureResult(IEnumerable<IError> errors)
    {
        return errors.FirstOrDefault() switch
        {
            InvalidRequestError invalidRequestError
                => BuildValidationProblemResponse(invalidRequestError),
            EntityNotFoundError entityNotFoundError
                => BuildProblemResponse(StatusCodes.Status404NotFound, entityNotFoundError),
            UnprocessableEntityError unprocessableEntityError
                => BuildProblemResponse(StatusCodes.Status422UnprocessableEntity, unprocessableEntityError),
            UnauthorizedError unauthorizedError
                => BuildProblemResponse(StatusCodes.Status401Unauthorized, unauthorizedError),
            ConflictError conflictError
                => BuildProblemResponse(StatusCodes.Status409Conflict, conflictError),
            _
                => BuildProblemResponse(StatusCodes.Status500InternalServerError)
        };
    }
    private ObjectResult BuildValidationProblemResponse(InvalidRequestError invalidRequestError)
    {
        var modelStateDictionary = new ModelStateDictionary();

        invalidRequestError
            .ValidationErrors
            .ToList()
            .ForEach(error => modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage));

        var validationProblemResponse = ProblemDetailsFactory.CreateValidationProblemDetails(
            HttpContext,
            instance: HttpContext.Request.Path, 
            modelStateDictionary: modelStateDictionary) as ValidationProblemResponse;

        return new ObjectResult(validationProblemResponse) { StatusCode = validationProblemResponse!.Status };
    }

    private ObjectResult BuildProblemResponse(int statusCode, IError error)
    {
        var problemResponse = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: statusCode,
            detail: error.Message,
            instance: HttpContext.Request.Path) as ProblemResponse;

        return new ObjectResult(problemResponse) { StatusCode = problemResponse!.Status };
    }

    private ObjectResult BuildProblemResponse(int statusCode)
    {
        var problemResponse = ProblemDetailsFactory.CreateProblemDetails(
        HttpContext,
           statusCode: statusCode,
           detail: "MyFinance API went rogue! Sorry!",
           instance: HttpContext.Request.Path) as ProblemResponse;

        return new ObjectResult(problemResponse) { StatusCode = problemResponse!.Status };
    }
}