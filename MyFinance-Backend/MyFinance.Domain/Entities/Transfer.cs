using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities
{
    public class Transfer : Entity
    {
        public Transfer(
            Guid businessUnitId, 
            string relatedTo, 
            string description, 
            double value, 
            DateTime settlementDate,
            TransferType tranferType)
        {
            BusinessUnitId = businessUnitId;
            RelatedTo = relatedTo;
            Description = description;
            Value = value;
            SettlementDate = settlementDate;
            Type = tranferType;
        }

        public Guid BusinessUnitId { get; private set; }
        public string RelatedTo { get; private set; }
        public string Description { get; private set; }
        public double Value { get; private set; }
        public DateTime SettlementDate { get; private set; }
        public TransferType Type { get; private set; }

        public void Update(
            string relatedTo, 
            string description, 
            double value, 
            DateTime settlementDate,
            TransferType tranferType) 
        {
            SetUpdateDate();
            RelatedTo = relatedTo;
            Description = description;
            Value = value;
            SettlementDate = settlementDate;
            Type = tranferType;
        }
    }
}
