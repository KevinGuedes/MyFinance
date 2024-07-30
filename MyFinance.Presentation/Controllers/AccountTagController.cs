using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.UnarchiveAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;
using MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;
using MyFinance.Contracts.AccountTag.Requests;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Contracts.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Account Tags management")]
[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
public class AccountTagController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new Account Tag")]
    [SwaggerResponse(StatusCodes.Status201Created, "Account Tag registered", typeof(AccountTagResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> RegisterAccountTagAsync(
        [FromBody] [SwaggerRequestBody("Account Tag's payload", Required = true)]
        CreateAccountTagRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CreateAccountTagCommand(request), cancellationToken);
        return ProcessResult(result, true);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all Account Tags with pagination")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of all existing Account Tags with pagination",
        typeof(Paginated<AccountTagResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> GetAccountTagsAsync(
        [FromQuery] [SwaggerParameter("Management Unit Id", Required = true)]
        Guid managementUnitId,
        [FromQuery] [SwaggerParameter("Page number", Required = true)]
        int pageNumber,
        [FromQuery] [SwaggerParameter("Units per page", Required = true)]
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetAccountTagsQuery(managementUnitId, pageNumber, pageSize);
        var result = await _sender.Send(query, cancellationToken);
        return ProcessResult(result);
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Account Tag")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Account Tag", typeof(AccountTagResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Account Tag not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Account Tag was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> UpdateAccountTagAsync(
        [FromBody] [SwaggerRequestBody("Account Tag payload", Required = true)]
        UpdateAccountTagRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new UpdateAccountTagCommand(request), cancellationToken);
        return ProcessResult(result);
    }

    [HttpPatch("{id:guid}")]
    [SwaggerOperation(Summary = "Unarchives an existing Account Tag")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Account Tag successfully unarchived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Account Tag Id", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Account Tag not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Account Tag was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> UnarchiveAccountTagAsync(
        [FromRoute] [SwaggerParameter("Id of the Account Tag to unarchive", Required = true)]
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new UnarchiveAccountTagCommand(id), cancellationToken);
        return ProcessResult(result);
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Logically deletes (archives) an existing Account Tag")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Account Tag successfully archived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Account Tag not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Account Tag was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> ArchiveAccountTagAsync(
        [FromBody] [SwaggerRequestBody("Payload to archvie a Account Tag", Required = true)]
        ArchiveAccountTagRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ArchiveAccountTagCommand(request), cancellationToken);
        return ProcessResult(result);
    }
}