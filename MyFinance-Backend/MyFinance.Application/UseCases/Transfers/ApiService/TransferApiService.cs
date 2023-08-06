using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.MappingProfiles;
using MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.UseCases.Transfers.DTOs;

namespace MyFinance.Application.UseCases.Transfers.ApiService;

public class TransferApiService : BaseApiService, ITransferApiService
{
    public TransferApiService(IMediator mediator) : base(mediator)
    {
    }

    public async Task<Result<TransferDTO>> RegisterTransfersAsync(
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
