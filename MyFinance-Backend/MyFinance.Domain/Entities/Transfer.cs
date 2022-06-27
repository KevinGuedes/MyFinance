using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities
{
    public class Transfer : Entity
    {
        public double Value { get; private set; }
        public string RelatedTo { get; private set; }
        public string Description { get; private set; }
        public DateTime SettlementDate { get; private set; }
        public TransferType Type { get; private set; }

        public Transfer(
           string relatedTo,
           string description,
           double value,
           DateTime settlementDate,
           TransferType tranferType)
        {
            Value = value;
            RelatedTo = relatedTo;
            Description = description;
            SettlementDate = settlementDate;
            Type = tranferType;
        }


        public void Update(
            string relatedTo,
            string description,
            double value,
            DateTime settlementDate,
            TransferType tranferType)
        {
            SetUpdateDate();
            Value = value;
            RelatedTo = relatedTo;
            Description = description;
            SettlementDate = settlementDate;
            Type = tranferType;
        }
    }
}
