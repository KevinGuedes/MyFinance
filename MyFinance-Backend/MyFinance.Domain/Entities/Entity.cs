namespace MyFinance.Domain.Entities;

public abstract class Entity
{
    //public byte[] RowVersion { get; private set; } = null!;

    private protected Entity()
        => CreationDate = DateTime.UtcNow;

    public Guid Id { get; }
    public DateTime CreationDate { get; private set; }
    public DateTime? UpdateDate { get; private set; }

    private protected void SetUpdateDateToNow()
        => UpdateDate = DateTime.UtcNow;
}