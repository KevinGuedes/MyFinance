using FluentResults;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.UseCases.Transfers.DTOs;

namespace MyFinance.Application.Abstractions.ApiServices;

public interface ITransferApiService
{
    Task<Result<TransferDTO>> RegisterTransferAsync(RegisterTransferCommand command, CancellationToken cancellationToken);
    Task<Result<TransferDTO>> UpdateTransferAsync(UpdateTransferCommand command, CancellationToken cancellationToken);
    Task<Result> DeleteTransferAsync(Guid Id, CancellationToken cancellationToken);
}
