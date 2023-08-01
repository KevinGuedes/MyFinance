using MyFinance.Application.Common.DTO;
using MyFinance.Application.UseCases.Transfers.DTOs;

namespace MyFinance.Application.UseCases.MonthlyBalances.DTOs;

public sealed class MonthlyBalanceDTO : BaseDTO
{
    public double Income { get; set; }
    public double Outcome { get; set; }
    public double Balance { get; set; }
    public int ReferenceMonth { get; set; }
    public int ReferenceYear { get; set; }
    public Guid BusinessUnitId { get; set; }

    public MonthlyBalanceDTO(
        double income,
        double outcome,
        double balance,
        int referenceMonth,
        int referenceYear,
        Guid businessUnitId)
    {
        Income = income;
        Outcome = outcome;
        Balance = balance;
        ReferenceMonth = referenceMonth;
        ReferenceYear = referenceYear;
        BusinessUnitId = businessUnitId;
    }
}
