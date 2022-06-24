using MediatR;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Transfers.Commands.DeleteTransfer
{
    public sealed class DeleteTransferCommand : IRequest, ICommand
    {
        public Guid TransferId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public DeleteTransferCommand(Guid transferId, int month, int year)
            => (TransferId, Month, Year) = (transferId, month, year);
    }
}
