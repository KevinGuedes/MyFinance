﻿using MyFinance.Application.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.Transfers.ViewModels;

namespace MyFinance.Application.Transfers.ApiService
{
    public interface ITransferApiService
    {
        Task RegisterTransfersAsync(RegisterTransfersCommand command, CancellationToken cancellationToken);
        Task<TransferViewModel> UpdateTransferAsync(UpdateTransferCommand command, CancellationToken cancellationToken);
        Task DeleteTransferAsync(DeleteTransferCommand command, CancellationToken cancellationToken);
    }
}
