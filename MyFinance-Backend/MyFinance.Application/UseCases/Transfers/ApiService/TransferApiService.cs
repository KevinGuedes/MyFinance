using AutoMapper;
using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.UseCases.Transfers.DTOs;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Transfers.ApiService;

public class TransferApiService : EntityApiService, ITransferApiService
{
    public TransferApiService(IMediator mediator, IMapper mapper)
         : base(mediator, mapper)
    {
    }

    public Task<Result> RegisterTransfersAsync(
        RegisterTransfersCommand command,
        CancellationToken cancellationToken)
        => _mediator.Send(command, cancellationToken);

    public async Task<Result<TransferDTO>> UpdateTransferAsync(
        UpdateTransferCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult<Transfer, TransferDTO>(result);
    }

    public Task<Result> DeleteTransferAsync(
        DeleteTransferCommand command,
        CancellationToken cancellationToken)
        => _mediator.Send(command, cancellationToken);
}
