using FluentResults;
using MediatR;
using MyFinance.Application.Abstractions.ApiServices;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.UseCases.Transfers.DTOs;

namespace MyFinance.Application.UseCases.Transfers.ApiService;

public sealed class TransferApiService(IMediator mediator) : BaseApiService(mediator), ITransferService
{
    public async Task<Result<TransferDTO>> RegisterTransferAsync(
        RegisterTransferCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult(result, DomainToDTOMapper.TransferToDTO);
    }

    public async Task<Result<TransferDTO>> UpdateTransferAsync(
        UpdateTransferCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult(result, DomainToDTOMapper.TransferToDTO);
    }

    public Task<Result> DeleteTransferAsync(Guid id, CancellationToken cancellationToken)
        => _mediator.Send(new DeleteTransferCommand(id), cancellationToken);
}