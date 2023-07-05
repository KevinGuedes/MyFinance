using System.Globalization;

namespace MyFinance.Domain.ValueObjects
{
    public class ReferenceData : ValueObject, IComparable<ReferenceData>
    {
        public Guid BusinessUnitId { get; init; }
        public DateTime Date { get; init; }
        public int Year { get => Date.Year; }
        public int Month { get => Date.Month; }

        protected ReferenceData () { }

        public ReferenceData(Guid businessUnitId, int year, int month)
        {
            BusinessUnitId = businessUnitId;
            Date = new DateTime(year, month, 1);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return BusinessUnitId;
            yield return Date;
        }

        public int CompareTo(ReferenceData? other)
        {
            if (other is null) return 1;
            return Date.CompareTo(other.Date);
        }

        public override string ToString()
           => string.Format(
                    "Reference [{0} - Business Unit Id: {1}]", 
                    Date.ToString("MMMM, YYYY", CultureInfo.InvariantCulture), 
                    BusinessUnitId
               );
    }
}
