using MyFinance.Application.Transfers.ViewModels;

namespace MyFinance.Application.MonthlyBalances.ViewModels
{
    public class MonthlyBalanceViewModel
    {
        public IEnumerable<TransferViewModel> Transfers { get; set; }
        public double CurrentBalance { get; set; }
        public Guid BusinessUnitId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public MonthlyBalanceViewModel(
            IEnumerable<TransferViewModel> transfers, 
            double currentBalance, 
            Guid businessUnitId, 
            int month, 
            int year)
        {
            Transfers = transfers;
            CurrentBalance = currentBalance;
            BusinessUnitId = businessUnitId;
            Month = month;
            Year = year;
        }
    }
}
