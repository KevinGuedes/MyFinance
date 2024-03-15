using MyFinance.Application.Common.DTO;

namespace MyFinance.Application.UseCases.BusinessUnits.DTOs;

public sealed class BusinessUnitDTO : EntityDTO
{
    public required string Name { get; init; }
    public required double Income { get; init; }
    public required double Outcome { get; init; }
    public required double Balance { get; init; }
    public required string? Description { get; init; }
    public required bool IsArchived { get; init; }
    public required string? ReasonToArchive { get; init; }
    public required DateTime? ArchiveDate { get; init; }
}