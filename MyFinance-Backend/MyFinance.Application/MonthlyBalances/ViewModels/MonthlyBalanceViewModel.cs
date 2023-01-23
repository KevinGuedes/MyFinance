using MyFinance.Application.Transfers.ViewModels;

namespace MyFinance.Application.MonthlyBalances.ViewModels
{
    public class MonthlyBalanceViewModel
    {
        public Guid Id { get; set; }
        public ReferenceDataViewModel ReferenceData { get; set; }
        public IEnumerable<TransferViewModel> Transfers { get; set; }
        public double CurrentBalance { get; set; }

        public MonthlyBalanceViewModel(
            Guid id,
            ReferenceDataViewModel referenceData,
            IEnumerable<TransferViewModel> transfers,
            double currentBalance)
        {
            Id = id;
            Transfers = transfers;
            CurrentBalance = currentBalance;
            ReferenceData = referenceData;
        }
    }
}
