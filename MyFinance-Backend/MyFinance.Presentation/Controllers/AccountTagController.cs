using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Abstractions.ApiServices;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;
using MyFinance.Application.UseCases.AccountTags.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Create and update Account Tags")]
public class AccountTagController(IAccountTagApiService accountTagApiService) : ApiController
{
    private readonly IAccountTagApiService _accountTagApiService = accountTagApiService;

    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new Account Tag")]
    [SwaggerResponse(StatusCodes.Status201Created, "Account Tag registered", typeof(AccountTagDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    public async Task<IActionResult> RegisterTransfersAsync(
        [FromBody] [SwaggerRequestBody("Account Tag's payload", Required = true)]
        CreateAccountTagCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _accountTagApiService.CreateAccountTagAsync(command, cancellationToken), true);

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all Account Tags with pagination")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of all existing Account Tags with pagination",
        typeof(IEnumerable<AccountTagDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(BadRequestResponse))]
    public async Task<IActionResult> GetBusinessUnitsAsync(
        [FromQuery] [SwaggerParameter("Page number", Required = true)]
        int page,
        [FromQuery] [SwaggerParameter("Units per page", Required = true)]
        int pageSize,
        CancellationToken cancellationToken)
        => ProcessResult(await _accountTagApiService.GetAccountTagsAsync(page, pageSize, cancellationToken));

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Account Tag")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Account Tag", typeof(AccountTagDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Account Tag not found", typeof(EntityNotFoundResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Account Tag was updated after the request was sent",
        typeof(UnprocessableEntityResponse))]
    public async Task<IActionResult> UpdateBusinessUnitAsync(
        [FromBody] [SwaggerRequestBody("Account Tag payload", Required = true)]
        UpdateAccountTagCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _accountTagApiService.UpdateAccountTagAsync(command, cancellationToken));

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
        => ProcessResult(await _accountTagApiService.UnarchiveAccountTagAsync(id, cancellationToken));

    [HttpDelete]
    [SwaggerOperation(Summary = "Logically deletes (archives) an existing Account Tag")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Account Tag successfully archived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Account Tag not found", typeof(EntityNotFoundResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Account Tag was updated after the request was sent",
        typeof(UnprocessableEntityResponse))]
    public async Task<IActionResult> ArchiveBusinessUnitAsync(
        [FromBody] [SwaggerRequestBody("Payload to archvie a Account Tag", Required = true)]
        ArchiveAccountTagCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _accountTagApiService.ArchiveAccountTagAsync(command, cancellationToken));
}