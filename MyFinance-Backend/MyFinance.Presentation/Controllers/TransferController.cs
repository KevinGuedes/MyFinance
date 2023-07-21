using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.Transfers.ApiService;
using MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.UseCases.Transfers.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Create, update and delete Transfers")]
public class TransferController : BaseController
{
    private readonly ITransferApiService _transferApiService;

    public TransferController(ITransferApiService transferApiService)
        => _transferApiService = transferApiService;

    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new Transfer")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    public async Task<IActionResult> RegisterTransfersAsync(
        [FromBody, SwaggerRequestBody("Transfers' payload", Required = true)] RegisterTransfersCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _transferApiService.RegisterTransfersAsync(command, cancellationToken));

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Transfer")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Transfer", typeof(TransferDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    public async Task<IActionResult> UpdateTransferAsync(
        [FromBody, SwaggerRequestBody("Transfers' payload", Required = true)] UpdateTransferCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _transferApiService.UpdateTransferAsync(command, cancellationToken));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Deletes an existing Transfer")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Transfer successfully deleted")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Transfer Id", typeof(BadRequestResponse))]
    public async Task<IActionResult> DeleteTransferAsync(
        [FromRoute, SwaggerParameter("Transfer Id", Required = true)] Guid id,
        CancellationToken cancellationToken)
        => ProcessResult(await _transferApiService.DeleteTransferAsync(id, cancellationToken));
}
