using FluentResults;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.UseCases.Transfers.DTOs;

namespace MyFinance.Application.UseCases.Transfers.ApiService;

public interface ITransferApiService
{
    Task<Result<TransferDTO>> RegisterTransfersAsync(RegisterTransfersCommand command, CancellationToken cancellationToken);
    Task<Result<TransferDTO>> UpdateTransferAsync(UpdateTransferCommand command, CancellationToken cancellationToken);
    Task<Result> DeleteTransferAsync(Guid Id, CancellationToken cancellationToken);
}
