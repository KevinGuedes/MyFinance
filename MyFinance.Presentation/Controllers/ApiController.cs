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
[SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(ProblemResponse))]
public abstract class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender _sender = sender;

    protected ObjectResult ProcessResult<TResponse>(Result<TResponse> result, bool hasEntityBeenCreated = false)
    {
        if (result.IsFailed)
            return HandleFailureResult(result.Errors);

        return hasEntityBeenCreated ?
            StatusCode(StatusCodes.Status201Created, result.Value) :
            Ok(result.Value);
    }

    protected IActionResult ProcessResult(Result result)
        => result.IsSuccess ? NoContent() : HandleFailureResult(result.Errors);

    protected ObjectResult HandleFailureResult(IEnumerable<IError> errors)
        => HandleFailureResult(errors.FirstOrDefault());

    protected ObjectResult HandleFailureResult(IError? error)
        => error switch
        {
            BadRequestError badRequest
                => BuildValidationProblemResponse(badRequest),
            EntityNotFoundError entityNotFoundError
                => BuildProblemResponse(StatusCodes.Status404NotFound, entityNotFoundError),
            UnprocessableEntityError unprocessableEntityError
                => BuildProblemResponse(StatusCodes.Status422UnprocessableEntity, unprocessableEntityError),
            UnauthorizedError unauthorizedError
                => BuildProblemResponse(StatusCodes.Status401Unauthorized, unauthorizedError),
            ConflictError conflictError
                => BuildProblemResponse(StatusCodes.Status409Conflict, conflictError),
            InternalServerError internalServerError
                => BuildProblemResponse(StatusCodes.Status500InternalServerError, internalServerError),
            null
                => BuildProblemResponse(StatusCodes.Status500InternalServerError),
            _
                => BuildProblemResponse(StatusCodes.Status500InternalServerError)
        };

    private ObjectResult BuildValidationProblemResponse(BadRequestError badRequestError)
    {
        var modelStateDictionary = new ModelStateDictionary();

        badRequestError
            .ValidationErrors
            .ToList()
            .ForEach(error => modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage));

        var validationProblemDetails = ProblemDetailsFactory.CreateValidationProblemDetails(
            HttpContext,
            modelStateDictionary,
            StatusCodes.Status400BadRequest,
            detail: "Invalid payload data, check the errors for more information",
            instance: HttpContext.Request.Path);

        return new(new ValidationProblemResponse(validationProblemDetails))
        {
            StatusCode = validationProblemDetails.Status
        };
    }

    private ObjectResult BuildProblemResponse(int statusCode, IError error)
    {
        var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: statusCode,
            detail: error.Message,
            instance: HttpContext.Request.Path);

        return new(new ProblemResponse(problemDetails))
        {
            StatusCode = problemDetails!.Status
        };
    }

    private ObjectResult BuildProblemResponse(int statusCode)
    {
        var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: statusCode,
            detail: "MyFinance API went rogue! Sorry!",
            instance: HttpContext.Request.Path);

        return new(new ProblemResponse(problemDetails))
        {
            StatusCode = problemDetails!.Status
        };
    }
}