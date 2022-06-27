using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Transfers.ApiService;
using MyFinance.Application.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.Transfers.Commands.UpdateTransfer;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferApiService _transferApiService;

        public TransferController(ITransferApiService transferApiService)
            => _transferApiService = transferApiService;

        [HttpPost]
        public async Task<IActionResult> CreateTransferAsync(
            [FromBody] RegisterTransfersCommand command, 
            CancellationToken cancellationToken)
        {
            await _transferApiService.RegisterTransfersAsync(command, cancellationToken);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransferAsync(
            [FromBody] UpdateTransferCommand command, 
            CancellationToken cancellationToken)
            => Ok(await _transferApiService.UpdateTransferAsync(command, cancellationToken));

        [HttpDelete]
        public async Task<IActionResult> DeleteTransferAsync(
            [FromBody] DeleteTransferCommand command, 
            CancellationToken cancellationToken)
        {
            await _transferApiService.DeleteTransferAsync(command, cancellationToken);
            return NoContent();
        }
    }
}
