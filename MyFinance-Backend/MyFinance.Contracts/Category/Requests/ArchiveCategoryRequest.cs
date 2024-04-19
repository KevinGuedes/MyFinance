namespace MyFinance.Contracts.Category.Requests;

public sealed record ArchiveCategoryRequest(Guid Id, string? ReasonToArchive)
{
}