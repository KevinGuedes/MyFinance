using MyFinance.Domain.Abstractions;

namespace MyFinance.Domain.Common;

public abstract class Entity : IAuditableEntity
{
    private protected Entity()
    {
    }

    public Guid Id { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUtc { get; private set; }
    //public byte[] RowVersion { get; private set; } = null!;

    public void SetUpdateOnToUtcNow()
        => UpdatedOnUtc = DateTime.UtcNow;

    public void SetUpdateOnToUtcNow(DateTime dateTime)
        => UpdatedOnUtc = dateTime;
}