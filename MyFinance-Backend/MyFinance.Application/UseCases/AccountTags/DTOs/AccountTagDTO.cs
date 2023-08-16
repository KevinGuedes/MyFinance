using MyFinance.Application.Common.DTO;

namespace MyFinance.Application.UseCases.AccountTags.DTOs;

public sealed class AccountTagDTO : BaseDTO
{
    public required string Tag { get; init; }
    public required string? Description { get; init; }
    public required bool IsArchived { get; init; }
    public required string? ReasonToArchive { get; init; }
    public required DateTime? ArchiveDate { get; init; }
}
