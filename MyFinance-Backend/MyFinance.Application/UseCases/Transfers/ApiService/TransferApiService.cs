using AutoMapper;
using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.UseCases.Transfers.DTOs;

namespace MyFinance.Application.UseCases.Transfers.ApiService;

public class TransferApiService : BaseApiService, ITransferApiService
{
    private readonly IMapper _mapper;
    public TransferApiService(IMediator mediator, IMapper mapper) : base(mediator)
        => _mapper = mapper;

    public async Task<Result<TransferDTO>> RegisterTransfersAsync(
        RegisterTransfersCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult(result, _mapper.Map<TransferDTO>);
    }

    public async Task<Result<TransferDTO>> UpdateTransferAsync(
        UpdateTransferCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult(result, _mapper.Map<TransferDTO>);
    }

    public Task<Result> DeleteTransferAsync(Guid id, CancellationToken cancellationToken)
        => _mediator.Send(new DeleteTransferCommand(id), cancellationToken);
}
