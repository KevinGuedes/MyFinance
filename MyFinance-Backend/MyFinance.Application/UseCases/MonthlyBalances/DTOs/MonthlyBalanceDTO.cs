using MyFinance.Application.Common.DTO;

namespace MyFinance.Application.UseCases.MonthlyBalances.DTOs;

public sealed class MonthlyBalanceDTO : BaseDTO
{
    public double Income { get; set; }
    public double Outcome { get; set; }
    public int ReferenceMonth { get; set; }
    public int ReferenceYear { get; set; }
    public Guid BusinessUnitId { get; set; }

    public MonthlyBalanceDTO(
        Guid id,
        double income,
        double outcome,
        int referenceMonth,
        int referenceYear,
        Guid businessUnitId)
    {
        Id = id;
        Income = income;
        Outcome = outcome;
        ReferenceMonth = referenceMonth;
        ReferenceYear = referenceYear;
        BusinessUnitId = businessUnitId;
    }
}
