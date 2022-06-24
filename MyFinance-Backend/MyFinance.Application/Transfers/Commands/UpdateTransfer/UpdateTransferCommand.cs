using MediatR;
using MyFinance.Application.Interfaces;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Transfers.Commands.UpdateTransfer
{
    public sealed class UpdateTransferCommand : IRequest<Transfer>, ICommand
    {
        public Guid CurrentMonthlyBalanceId { get; set; }
        public double AbsoluteValue { get; set; }
        public string RelatedTo { get; set; }
        public string Description { get; set; }
        public DateTime SettlementDate { get; set; }
        public TransferType Type { get; set; }

        public UpdateTransferCommand(
            Guid currentMonthlyBalanceId,
            double absoluteValue,
            string relatedTo,
            string description,
            DateTime settlementDate,
            TransferType type)
        {
            CurrentMonthlyBalanceId = currentMonthlyBalanceId;
            AbsoluteValue = absoluteValue;
            RelatedTo = relatedTo;
            Description = description;
            SettlementDate = settlementDate;
            Type = type;
        }
    }
}
