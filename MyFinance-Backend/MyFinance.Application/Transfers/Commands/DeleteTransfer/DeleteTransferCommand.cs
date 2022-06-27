using MediatR;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Transfers.Commands.DeleteTransfer
{
    public sealed class DeleteTransferCommand : IRequest, ICommand
    {
        public Guid MonthlyBalanceId { get; set; }
        public Guid TransferId { get; set; }

        public DeleteTransferCommand(Guid monthlyBalanceId, Guid transferId)
            => (MonthlyBalanceId, TransferId) = (monthlyBalanceId, transferId);
    }
}
