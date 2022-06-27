using MyFinance.Domain.Enums;

namespace MyFinance.Application.Transfers.ViewModels
{
    public class TransferViewModel
    {
        public Guid Id { get; set; }
        public string RelatedTo { get; set; }
        public string Description { get; set; }
        public DateTime SettlementDate { get; set; }
        public TransferType Type { get; set; }
        public double Value { get; set; }

        public TransferViewModel(
            Guid id,
            string relatedTo, 
            string description, 
            DateTime settlementDate, 
            TransferType type, 
            double value)
        {
            Id = id;
            RelatedTo = relatedTo;
            Description = description;
            SettlementDate = settlementDate;
            Type = type;
            Value = value;
        }
    }
}
