namespace MyFinance.Domain.ValueObjects
{
    public class ReferenceData : ValueObject
    {
        public Guid BusinessUnitId { get; set; }
        public DateTime Date { get; set; }
        public int Year { get => Date.Year; }
        public int Month { get => Date.Month; }

        public ReferenceData(Guid businessUnitId, int month, int year)
        {
            BusinessUnitId = businessUnitId;
            Date = new DateTime(year, month, 1);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Month;
            yield return Year;
            yield return BusinessUnitId;
        }
    }
}
