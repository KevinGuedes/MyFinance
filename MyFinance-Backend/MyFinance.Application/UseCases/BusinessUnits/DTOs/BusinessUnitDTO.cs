using MyFinance.Application.Common.DTO;

namespace MyFinance.Application.UseCases.BusinessUnits.DTOs;

public sealed class BusinessUnitDTO : BaseDTO
{
    public string Name { get; set; }
    public double Income { get; set; }
    public double Outcome { get; set; }
    public double Balance { get; set; }
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
    public string? ReasonToArchive { get; set; }
    public DateTime? ArchiveDate { get; set; }

    public BusinessUnitDTO(
        string name,
        double income,
        double outcome,
        double balance,
        string? description,
        bool isArchived,
        string? reasonToArchive,
        DateTime? archiveDate)
    {
        Name = name;
        Income = income;
        Outcome = outcome;
        Balance = balance;
        Description = description;
        IsArchived = isArchived;
        ReasonToArchive = reasonToArchive;
        ArchiveDate = archiveDate;
    }
}
