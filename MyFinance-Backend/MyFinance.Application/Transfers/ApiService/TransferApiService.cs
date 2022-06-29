using AutoMapper;
using MediatR;
using MyFinance.Application.Generics;
using MyFinance.Application.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.Transfers.Commands.RegisterTransfers;
using MyFinance.Application.Transfers.Commands.UpdateTransfer;
using MyFinance.Application.Transfers.ViewModels;

namespace MyFinance.Application.Transfers.ApiService
{
    public class TransferApiService : EntityApiService, ITransferApiService
    {
        public TransferApiService(IMediator mediator, IMapper mapper)
             : base(mediator, mapper)
        {
        }

        public Task RegisterTransfersAsync(RegisterTransfersCommand command, CancellationToken cancellationToken)
             => _mediator.Send(command, cancellationToken);

        public async Task<TransferViewModel> UpdateTransferAsync(UpdateTransferCommand command, CancellationToken cancellationToken)
            => _mapper.Map<TransferViewModel>(await _mediator.Send(command, cancellationToken));

        public Task DeleteTransferAsync(DeleteTransferCommand command, CancellationToken cancellationToken)
             => _mediator.Send(command, cancellationToken);
    }
}
