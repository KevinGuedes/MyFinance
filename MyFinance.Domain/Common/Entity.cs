using MyFinance.Domain.Abstractions;

namespace MyFinance.Domain.Common;

public abstract class Entity : IAuditableEntity
{
    public Guid Id { get; private init; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUtc { get; private set; }

    public void SetUpdateOnToUtcNow()
        => UpdatedOnUtc = DateTime.UtcNow;

    public void SetUpdatedOnTo(DateTime updatedOnUtc)
        => UpdatedOnUtc = updatedOnUtc;
}