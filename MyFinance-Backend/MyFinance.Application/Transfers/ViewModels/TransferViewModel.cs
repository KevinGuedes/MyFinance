using MyFinance.Domain.Enums;

namespace MyFinance.Application.Transfers.ViewModels
{
    public class TransferViewModel
    {
        public string RelatedTo { get; set; }
        public string Description { get; set; }
        public DateTime SettlementDate { get; set; }
        public TransferType Type { get; set; }
        public double Value { get; set; }

        public TransferViewModel(
            string relatedTo, 
            string description, 
            DateTime settlementDate, 
            TransferType type, 
            double value)
        {
            RelatedTo = relatedTo;
            Description = description;
            SettlementDate = settlementDate;
            Type = type;
            Value = value;
        }
    }
}
