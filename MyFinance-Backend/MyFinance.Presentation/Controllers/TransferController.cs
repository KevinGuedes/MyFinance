using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Transfers.ApiService;
using MyFinance.Application.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.Transfers.Commands.DeleteTransfer;

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
        public async Task<IActionResult> CreateTransferAsync(RegisterTransfersCommand command, CancellationToken cancellationToken)
        {
            await _transferApiService.RegisterTransfersAsync(command, cancellationToken);
            return NoContent();
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteTransferIdAsync(DeleteTransferByIdCommand command, CancellationToken cancellationToken)
        //{
        //    await _transferApiService.DeleteTransferByIdAsync(command, cancellationToken);
        //    return NoContent();
        //}

        [HttpDelete]
        public async Task<IActionResult> UpdateTransferById(DeleteTransferCommand command, CancellationToken cancellationToken)
        {
            await _transferApiService.DeleteTransferByIdAsync(command, cancellationToken);
            return NoContent();
        }
    }
}
