namespace MyFinance.Contracts.AccountTag.Requests;

public class ArchiveAccountTagRequest
{
    public required Guid Id { get; init; }
    public required string? ReasonToArchive { get; init; }
}