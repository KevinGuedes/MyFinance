using FluentResults;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.Common.Errors;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(InternalServerErrorResponse))]
public abstract class ApiController : ControllerBase
{
    protected IActionResult ProcessResult<TResponse>(Result<TResponse> result, bool isCreatedEntity = false)
    {
        if (result.IsSuccess)
        {
            return isCreatedEntity ?
                StatusCode(StatusCodes.Status201Created, result.Value) :
                Ok(result.Value);
        }

        return HandleFailureResult(result.Errors);
    }

    protected IActionResult ProcessResult(Result result)
    {
        if (result.IsSuccess)
            return NoContent();
        return HandleFailureResult(result.Errors);
    }

    protected IActionResult ProcessFileResult(Result<Tuple<string, byte[]>> result, string contentType)
    {
        if (result.IsFailed)
            return HandleFailureResult(result.Errors);

        var (fileName, fileContent) = result.Value;
        return File(fileContent, contentType, fileName, true);
    }

    protected IActionResult HandleFailureResult(List<IError> errors)
    {
        var invalidRequestError = errors.OfType<InvalidRequestError>().FirstOrDefault();
        if (invalidRequestError is not null)
            return BuildBadRequestResponse(invalidRequestError);

        var entityNotFoundError = errors.OfType<EntityNotFoundError>().FirstOrDefault();
        if (entityNotFoundError is not null)
            return BuildNotFoundResponse(entityNotFoundError);

        var unprocessableEntityError = errors.OfType<UnprocessableEntityError>().FirstOrDefault();
        if (unprocessableEntityError is not null)
            return BuildUnprocessableEntityResponse(unprocessableEntityError);

        var unauthorizedError = errors.OfType<UnauthorizedError>().FirstOrDefault();
        if (unauthorizedError is not null)
            return BuildUnauthorizedResponse(unauthorizedError);

        var conflictError = errors.OfType<ConflictError>().FirstOrDefault();
        if (conflictError is not null)
            return BuildConflictResponse(conflictError);

        var internalServerError = new InternalServerError();
        return BuildInternalServerErrorResponse(internalServerError);
    }

    private ConflictObjectResult BuildConflictResponse(ConflictError conflictError)
        => Conflict(new ConflictResponse(conflictError));

    private UnauthorizedObjectResult BuildUnauthorizedResponse(UnauthorizedError unauthorizedError)
        => Unauthorized(new UnauthorizedResponse(unauthorizedError));

    private BadRequestObjectResult BuildBadRequestResponse(InvalidRequestError invalidRequestError)
        => BadRequest(new BadRequestResponse(invalidRequestError));

    private NotFoundObjectResult BuildNotFoundResponse(EntityNotFoundError entityNotFoundError)
        => NotFound(new EntityNotFoundResponse(entityNotFoundError));

    private UnprocessableEntityObjectResult BuildUnprocessableEntityResponse(UnprocessableEntityError unprocessableEntityError)
        => UnprocessableEntity(new UnprocessableEntityResponse(unprocessableEntityError));

    protected IActionResult BuildInternalServerErrorResponse(InternalServerError internalServerError)
    {
        var internalServerErrorResponse = new InternalServerErrorResponse(internalServerError);
        return StatusCode(StatusCodes.Status500InternalServerError, internalServerErrorResponse);
    }
}
