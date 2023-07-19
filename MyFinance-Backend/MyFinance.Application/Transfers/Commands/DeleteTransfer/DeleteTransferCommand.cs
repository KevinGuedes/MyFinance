using MyFinance.Application.Common.RequestHandling;

namespace MyFinance.Application.Transfers.Commands.DeleteTransfer;

public sealed class DeleteTransferCommand : Command
{
    public Guid MonthlyBalanceId { get; set; }
    public Guid TransferId { get; set; }

    public DeleteTransferCommand(Guid monthlyBalanceId, Guid transferId)
        => (MonthlyBalanceId, TransferId) = (monthlyBalanceId, transferId);
}
