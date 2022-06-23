using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities
{
    public class Transfer : Entity
    {
        private double _absoluteValue;
        public string RelatedTo { get; private set; }
        public string Description { get; private set; }
        public DateTime SettlementDate { get; private set; }
        public TransferType Type { get; private set; }
        public double FormattedValue
        {
            get
            {
                if (Type == TransferType.Profit) return _absoluteValue;
                else return -_absoluteValue;
            }
        }

        public Transfer(
           string relatedTo,
           string description,
           double absoluteValue,
           DateTime settlementDate,
           TransferType tranferType)
        {
            _absoluteValue = absoluteValue;
            RelatedTo = relatedTo;
            Description = description;
            SettlementDate = settlementDate;
            Type = tranferType;
        }


        public void Update(
            string relatedTo,
            string description,
            double absoluteValue,
            DateTime settlementDate,
            TransferType tranferType)
        {
            SetUpdateDate();
            _absoluteValue = absoluteValue;
            RelatedTo = relatedTo;
            Description = description;
            SettlementDate = settlementDate;
            Type = tranferType;
        }

        public bool ShoudlGoToAnotherMonthlyBalance(DateTime newSettlementDate)
            => newSettlementDate.Month != SettlementDate.Month || newSettlementDate.Year != SettlementDate.Year;

        public bool ShouldUpdateBusinessUnitBalance(double newFormattedValue)
            => FormattedValue != newFormattedValue;
    }
}
