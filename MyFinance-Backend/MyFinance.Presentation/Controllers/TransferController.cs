using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Transfers.ApiService;
using MyFinance.Application.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.Transfers.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Register, update and delete Transfers")]
    public class TransferController : ControllerBase
    {
        private readonly ITransferApiService _transferApiService;

        public TransferController(ITransferApiService transferApiService)
            => _transferApiService = transferApiService;

        [HttpPost]
        [SwaggerOperation(Summary = "Registers a set of new Transfers")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RegisterTransfersAsync(
            [FromBody, SwaggerRequestBody("Transfers' payload", Required = true)] RegisterTransfersCommand command, 
            CancellationToken cancellationToken)
        {
            await _transferApiService.RegisterTransfersAsync(command, cancellationToken);
            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing Transfer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Updated Transfer", typeof(TransferViewModel))]
        public async Task<IActionResult> UpdateTransferAsync(
            [FromBody, SwaggerRequestBody("Transfers' payload", Required = true)] UpdateTransferCommand command, 
            CancellationToken cancellationToken)
            => Ok(await _transferApiService.UpdateTransferAsync(command, cancellationToken));

        [HttpDelete]
        [SwaggerOperation(Summary = "Deletes an existing Transfer")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTransferAsync(
            [FromBody, SwaggerRequestBody("Transfer data", Required = true)] DeleteTransferCommand command, 
            CancellationToken cancellationToken)
        {
            await _transferApiService.DeleteTransferAsync(command, cancellationToken);
            return NoContent();
        }
    }
}
