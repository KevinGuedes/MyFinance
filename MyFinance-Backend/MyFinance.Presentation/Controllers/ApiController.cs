using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
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

    protected ProblemDetails BuildProblemDetails(
        int statusCode, 
        string detail,
        IDictionary<string, object?>? extensions = null)
    {
        //.net 9 will have a Problem with extensions as parameter, and then this code will be simplified 
        // Maybe not if they dont add it to the problem fetails factory
        //https://github.com/dotnet/aspnetcore/pull/50204
        //https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.Core/src/ControllerBase.cs#L1839
        var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: statusCode,
            detail: detail,
            instance: HttpContext.Request.Path);

        problemDetails.Title ??= ReasonPhrases.GetReasonPhrase(statusCode);

        if (extensions is not null)
        {
            foreach (var extension in extensions)
            {
                problemDetails.Extensions.Add(extension);
            }
        }

        return problemDetails;
    }

    protected IActionResult HandleFailureResult(IEnumerable<IError> errors)
        => HandleFailureResult(errors.FirstOrDefault());

    protected IActionResult HandleFailureResult(IError? error)
    {
        if (error is null)
            return BuildProblemResponse(StatusCodes.Status500InternalServerError);

        return error switch
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

    private ActionResult BuildValidationProblemResponse(InvalidRequestError invalidRequestError)
    {
        var modelStateDictionary = new ModelStateDictionary();

        invalidRequestError
            .ValidationErrors
            .ToList()
            .ForEach(error => modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage));

        return ValidationProblem(instance: HttpContext.Request.Path, modelStateDictionary: modelStateDictionary);
    }

    private ObjectResult BuildProblemResponse(int statusCode, IError error)
        => Problem(statusCode: statusCode, detail: error.Message, instance: HttpContext.Request.Path);

    private ObjectResult BuildProblemResponse(int statusCode)
        => Problem(statusCode: statusCode, detail: "MyFinance API went rogue! Sorry!", instance: HttpContext.Request.Path);
}