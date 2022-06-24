using MyFinance.Domain.Enums;

namespace MyFinance.Application.Transfers.Commands.AddTransfer
{
    public sealed class CreateTransferCommand
    {
        public double AbsoluteValue { get; set; }
        public string RelatedTo { get; set; }
        public string Description { get; set; }
        public DateTime SettlementDate { get; set; }
        public TransferType Type { get; set; }

        public CreateTransferCommand(
            double absoluteValue, 
            string relatedTo, 
            string description, 
            DateTime settlementDate, 
            TransferType type)
        {
            AbsoluteValue = absoluteValue;
            RelatedTo = relatedTo;
            Description = description;
            SettlementDate = settlementDate;
            Type = type;
        }
    }
}
