using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.UseCases.Transfers.ApiService;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.UseCases.Transfers.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Create, update and delete Transfers")]
public class TransferController(ITransferApiService transferApiService) : ApiController
{
    private readonly ITransferApiService _transferApiService = transferApiService;

    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new Transfer")]
    [SwaggerResponse(StatusCodes.Status201Created, "Transfer registered", typeof(TransferDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(EntityNotFoundResponse))]
    public async Task<IActionResult> RegisterTransfersAsync(
        [FromBody, SwaggerRequestBody("Transfers' payload", Required = true)] RegisterTransferCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _transferApiService.RegisterTransferAsync(command, cancellationToken), true);

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Transfer")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Transfer", typeof(TransferDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Transfer not found", typeof(EntityNotFoundResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Transfer was updated after the request was sent", typeof(UnprocessableEntityResponse))]
    public async Task<IActionResult> UpdateTransferAsync(
        [FromBody, SwaggerRequestBody("Transfers' payload", Required = true)] UpdateTransferCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _transferApiService.UpdateTransferAsync(command, cancellationToken));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Deletes an existing Transfer")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Transfer successfully deleted")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Transfer Id", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Transfer not found", typeof(EntityNotFoundResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Transfer was updated after the request was sent", typeof(UnprocessableEntityResponse))]
    public async Task<IActionResult> DeleteTransferAsync(
        [FromRoute, SwaggerParameter("Transfer Id", Required = true)] Guid id,
        CancellationToken cancellationToken)
        => ProcessResult(await _transferApiService.DeleteTransferAsync(id, cancellationToken));
}
