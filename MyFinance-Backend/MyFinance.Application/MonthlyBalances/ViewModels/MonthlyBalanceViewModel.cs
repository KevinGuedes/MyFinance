using MyFinance.Application.Transfers.ViewModels;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.MonthlyBalances.ViewModels;

public class MonthlyBalanceViewModel
{
    public Guid Id { get; set; }
    public double Income { get; set; }
    public double Outcome { get; set; }
    public DateTime ReferenceDate { get; set; }
    public int ReferenceMonth { get; set; }
    public int ReferenceYear { get; set; }
    public Guid BusinessUnitId { get; set; }
    public virtual BusinessUnit BusinessUnit { get; set; }
    public virtual List<TransferViewModel> Transfers { get; set; }

    public MonthlyBalanceViewModel(
        Guid id,
        double income,
        double outcome,
        DateTime referenceDate,
        int referenceMonth,
        int referenceYear,
        Guid businessUnitId,
        BusinessUnit businessUnit,
        List<TransferViewModel> transfers)
    {
        Id = id;
        Income = income;
        Outcome = outcome;
        ReferenceDate = referenceDate;
        ReferenceMonth = referenceMonth;
        ReferenceYear = referenceYear;
        BusinessUnitId = businessUnitId;
        BusinessUnit = businessUnit;
        Transfers = transfers;
    }
}
