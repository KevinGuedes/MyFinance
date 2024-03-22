using MyFinance.Domain.Abstractions;

namespace MyFinance.Domain.Common;

public abstract class Entity : IAuditableEntity
{
    private protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    //public byte[] RowVersion { get; private set; } = null!;

    public void SetUpdateOnToUtcNow()
        => UpdatedOnUtc = DateTime.UtcNow;

    public void SetUpdateOnToUtcNow(DateTime dateTime)
        => UpdatedOnUtc = dateTime;
}