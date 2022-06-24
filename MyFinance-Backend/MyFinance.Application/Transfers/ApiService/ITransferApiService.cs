using MyFinance.Application.Transfers.Commands.AddTransfer;
using MyFinance.Application.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.Transfers.ViewModels;

namespace MyFinance.Application.Transfers.ApiService
{
    public interface ITransferApiService
    {
        Task<TransferViewModel> CreateTransferAsync(CreateTransferCommand command, CancellationToken cancellationToken);
        Task<TransferViewModel> UpdateTransferAsync(UpdateTransferCommand command, CancellationToken cancellationToken);
        Task DeleteTransferByIdAsync(DeleteTransferCommand command, CancellationToken cancellationToken);
    }
}
