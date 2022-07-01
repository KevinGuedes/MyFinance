using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Transfers.Commands.UpdateTransfer
{
    public sealed class UpdateTransferCommand : Command<Transfer>
    {
        public Guid TransferId { get; set; }
        public Guid CurrentMonthlyBalanceId { get; set; }
        public double Value { get; set; }
        public string RelatedTo { get; set; }
        public string Description { get; set; }
        public DateTime SettlementDate { get; set; }
        public TransferType TransferType { get; set; }

        public UpdateTransferCommand(
            Guid transferId,
            Guid currentMonthlyBalanceId,
            double value,
            string relatedTo,
            string description,
            DateTime settlementDate,
            TransferType transferType)
        {
            TransferId = transferId;
            CurrentMonthlyBalanceId = currentMonthlyBalanceId;
            Value = value;
            RelatedTo = relatedTo;
            Description = description;
            SettlementDate = settlementDate;
            TransferType = transferType;
        }
    }
}
