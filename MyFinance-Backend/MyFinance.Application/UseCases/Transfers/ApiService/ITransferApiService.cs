using FluentResults;
using MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.UseCases.Transfers.ViewModels;

namespace MyFinance.Application.UseCases.Transfers.ApiService;

public interface ITransferApiService
{
    Task<Result> RegisterTransfersAsync(RegisterTransfersCommand command, CancellationToken cancellationToken);
    Task<Result<TransferViewModel>> UpdateTransferAsync(UpdateTransferCommand command, CancellationToken cancellationToken);
    Task<Result> DeleteTransferAsync(DeleteTransferCommand command, CancellationToken cancellationToken);
}
