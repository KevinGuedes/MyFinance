namespace MyFinance.Domain.Abstractions;

public interface IAuditableEntity
{
    DateTime CreatedOnUtc { get; }
    DateTime? UpdatedOnUtc { get; }
    void SetUpdateOnToUtcNow();
    void SetUpdatedOnTo(DateTime dateTime);
}
