namespace MyFinance.Contracts.AccountTag.Requests;

public sealed record ArchiveAccountTagRequest(Guid Id, string? ReasonToArchive)
{
}