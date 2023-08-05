using MyFinance.Application.Common.DTO;
using MyFinance.Application.UseCases.MonthlyBalances.DTOs;

namespace MyFinance.Application.UseCases.BusinessUnits.DTOs;

public sealed class BusinessUnitDTO : BaseDTO
{
    public string Name { get; set; } = string.Empty;
    public double Income { get; set; }
    public double Outcome { get; set; }
    public double Balance { get; set; }
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
    public string? ReasonToArchive { get; set; }
    public DateTime? ArchiveDate { get; set; }
}
