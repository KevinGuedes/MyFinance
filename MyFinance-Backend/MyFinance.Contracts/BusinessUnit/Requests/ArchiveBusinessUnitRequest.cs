namespace MyFinance.Contracts.BusinessUnit.Requests;

public sealed record ArchiveBusinessUnitRequest(Guid Id, string? ReasonToArchive)
{
}