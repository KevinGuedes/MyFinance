using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.AccountTags.Commands.UnarchiveAccountTag;
using MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;
using MyFinance.Contracts.AccountTag;
using MyFinance.Contracts.AccountTag.Requests;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Contracts.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Create and update Account Tags")]
public class AccountTagController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new Account Tag")]
    [SwaggerResponse(StatusCodes.Status201Created, "Account Tag registered", typeof(AccountTagResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    public async Task<IActionResult> RegisterTransfersAsync(
        [FromBody] [SwaggerRequestBody("Account Tag's payload", Required = true)]
        CreateAccountTagRequest request,
        CancellationToken cancellationToken)
         => ProcessResult(await _mediator.Send(AccountTagMapper.RTC.Map(request), cancellationToken), true);

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all Account Tags with pagination")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of all existing Account Tags with pagination",
        typeof(PaginatedResponse<AccountTagResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(BadRequestResponse))]
    public async Task<IActionResult> GetBusinessUnitsAsync(
        [FromQuery] [SwaggerParameter("Page number", Required = true)]
        int pageNumber,
        [FromQuery] [SwaggerParameter("Units per page", Required = true)]
        int pageSize,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new GetAccountTagsQuery(pageNumber, pageSize), cancellationToken));

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Account Tag")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Account Tag", typeof(AccountTagResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Account Tag not found", typeof(EntityNotFoundResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Account Tag was updated after the request was sent",
        typeof(UnprocessableEntityResponse))]
    public async Task<IActionResult> UpdateBusinessUnitAsync(
        [FromBody] [SwaggerRequestBody("Account Tag payload", Required = true)]
        UpdateAccountTagRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(AccountTagMapper.RTC.Map(request), cancellationToken));

    [HttpPatch("{id:guid}")]
    [SwaggerOperation(Summary = "Unarchives an existing Account Tag")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Account Tag successfully unarchived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Account Tag Id", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Account Tag not found", typeof(EntityNotFoundResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Account Tag was updated after the request was sent",
        typeof(UnprocessableEntityResponse))]
    public async Task<IActionResult> UnarchiveBusinessUnitAsync(
        [FromRoute] [SwaggerParameter("Id of the Account Tag to unarchive", Required = true)]
        Guid id,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new UnarchiveAccountTagCommand(id), cancellationToken));

    [HttpDelete]
    [SwaggerOperation(Summary = "Logically deletes (archives) an existing Account Tag")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Account Tag successfully archived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Account Tag not found", typeof(EntityNotFoundResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Account Tag was updated after the request was sent",
        typeof(UnprocessableEntityResponse))]
    public async Task<IActionResult> ArchiveBusinessUnitAsync(
        [FromBody] [SwaggerRequestBody("Payload to archvie a Account Tag", Required = true)]
        ArchiveAccountTagRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(AccountTagMapper.RTC.Map(request), cancellationToken));
}