using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Requests;
using MyFinance.Contracts.Transfer.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Transfers management")]
[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
public class TransferController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new Transfer")]
    [SwaggerResponse(StatusCodes.Status201Created, "Transfer registered", typeof(TransferResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(ProblemResponse))]
    public async Task<IActionResult> RegisterTransfersAsync(
        [FromBody] [SwaggerRequestBody("Transfers' payload", Required = true)]
        RegisterTransferRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(TransferMapper.RTC.Map(request), cancellationToken), true);

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Transfer")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Transfer", typeof(TransferResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Transfer not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Transfer was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> UpdateTransferAsync(
        [FromBody] [SwaggerRequestBody("Transfers' payload", Required = true)]
        UpdateTransferRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(TransferMapper.RTC.Map(request), cancellationToken));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Deletes an existing Transfer")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Transfer successfully deleted")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Transfer Id", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Transfer not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Transfer was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> DeleteTransferAsync(
        [FromRoute] [SwaggerParameter("Transfer Id", Required = true)]
        Guid id,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new DeleteTransferCommand(id), cancellationToken));
}