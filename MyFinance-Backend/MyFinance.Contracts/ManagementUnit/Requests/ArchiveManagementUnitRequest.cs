namespace MyFinance.Contracts.ManagementUnit.Requests;

public sealed record ArchiveManagementUnitRequest(Guid Id, string? ReasonToArchive)
{
}