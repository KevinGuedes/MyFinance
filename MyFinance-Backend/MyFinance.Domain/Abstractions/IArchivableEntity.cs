namespace MyFinance.Domain.Abstractions;

public interface IArchivableEntity
{
    bool IsArchived { get; }
    DateTime? ArchivedOnUtc { get; }
    string? ReasonToArchive { get; }
    void Archive(string? reasonToArchive);
    void Unarchive();
}
