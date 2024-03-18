using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.Common.Errors;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(InternalServerErrorResponse))]
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
        => errors.FirstOrDefault() switch
            {
                InvalidRequestError invalidRequestError => BuildBadRequestResponse(invalidRequestError),
                EntityNotFoundError entityNotFoundError => BuildNotFoundResponse(entityNotFoundError),
                UnprocessableEntityError unprocessableEntityError => BuildUnprocessableEntityResponse(unprocessableEntityError),
                UnauthorizedError unauthorizedError => BuildUnauthorizedResponse(unauthorizedError),
                ConflictError conflictError => BuildConflictResponse(conflictError),
                _ => BuildInternalServerErrorResponse(new InternalServerError())
            };

    private ConflictObjectResult BuildConflictResponse(ConflictError conflictError)
        => Conflict(new ConflictResponse(conflictError));

    private UnauthorizedObjectResult BuildUnauthorizedResponse(UnauthorizedError unauthorizedError)
        => Unauthorized(new UnauthorizedResponse(unauthorizedError));

    private BadRequestObjectResult BuildBadRequestResponse(InvalidRequestError invalidRequestError)
        => BadRequest(new BadRequestResponse(invalidRequestError));

    private NotFoundObjectResult BuildNotFoundResponse(EntityNotFoundError entityNotFoundError)
        => NotFound(new EntityNotFoundResponse(entityNotFoundError));

    private UnprocessableEntityObjectResult BuildUnprocessableEntityResponse(
        UnprocessableEntityError unprocessableEntityError)
        => UnprocessableEntity(new UnprocessableEntityResponse(unprocessableEntityError));

    protected IActionResult BuildInternalServerErrorResponse(InternalServerError internalServerError)
        => StatusCode(
            StatusCodes.Status500InternalServerError, 
            new InternalServerErrorResponse(internalServerError));
}