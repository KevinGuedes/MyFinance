using MyFinance.Application.UseCases.Transfers.ViewModels;

namespace MyFinance.Application.UseCases.MonthlyBalances.ViewModels;

public class MonthlyBalanceViewModel
{
    public Guid Id { get; set; }
    public double Income { get; set; }
    public double Outcome { get; set; }
    public DateTime ReferenceDate { get; set; }
    public int ReferenceMonth { get; set; }
    public int ReferenceYear { get; set; }
    public Guid BusinessUnitId { get; set; }
    public List<TransferViewModel> Transfers { get; set; }

    public MonthlyBalanceViewModel(
        Guid id,
        double income,
        double outcome,
        DateTime referenceDate,
        int referenceMonth,
        int referenceYear,
        Guid businessUnitId,
        List<TransferViewModel> transfers)
    {
        Id = id;
        Income = income;
        Outcome = outcome;
        ReferenceDate = referenceDate;
        ReferenceMonth = referenceMonth;
        ReferenceYear = referenceYear;
        BusinessUnitId = businessUnitId;
        Transfers = transfers;
    }
}
