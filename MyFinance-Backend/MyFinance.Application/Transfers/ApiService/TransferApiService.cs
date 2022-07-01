using AutoMapper;
using FluentResults;
using MediatR;
using MyFinance.Application.Generics.ApiService;
using MyFinance.Application.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.Transfers.ViewModels;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Transfers.ApiService
{
    public class TransferApiService : EntityApiService, ITransferApiService
    {
        public TransferApiService(IMediator mediator, IMapper mapper)
             : base(mediator, mapper)
        {
        }

        public async Task<Result> RegisterTransfersAsync(RegisterTransfersCommand command, CancellationToken cancellationToken)
            => ProcessResult(await _mediator.Send(command, cancellationToken));

        public async Task<Result<TransferViewModel>> UpdateTransferAsync(UpdateTransferCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ProcessResultAndMapIfSuccess<Transfer, TransferViewModel>(result);
        }

        public async Task<Result> DeleteTransferAsync(DeleteTransferCommand command, CancellationToken cancellationToken)
            => ProcessResult(await _mediator.Send(command, cancellationToken));
    }
}
