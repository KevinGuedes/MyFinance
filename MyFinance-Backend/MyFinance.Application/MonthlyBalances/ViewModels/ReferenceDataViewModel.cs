namespace MyFinance.Application.MonthlyBalances.ViewModels
{
    public class ReferenceDataViewModel
    {
        public Guid BusinessUnitId { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public ReferenceDataViewModel(Guid businessUnitId, DateTime date, int year, int month)
            => (BusinessUnitId, Date, Year, Month) = (businessUnitId, date, year, month);
    }
}
