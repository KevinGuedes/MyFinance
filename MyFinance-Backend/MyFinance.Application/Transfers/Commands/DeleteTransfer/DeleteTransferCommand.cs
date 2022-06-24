using MediatR;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Transfers.Commands.DeleteTransfer
{
    public sealed class DeleteTransferCommand : IRequest, ICommand
    {
        public Guid MonhlyBalanceId { get; set; }
        public Guid BusinessUnitId { get; set; }
        public Guid TransferId { get; set; }

        public DeleteTransferCommand(Guid monthlyBalanceId, Guid businessUnitId, Guid transferId, int month, int year)
            => (MonhlyBalanceId, BusinessUnitId, TransferId) = (monthlyBalanceId, businessUnitId, transferId);
    }
}
