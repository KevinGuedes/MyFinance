namespace MyFinance.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime? UpdateDate { get; private set; }
    public byte[] RowVersion { get; private set; } = null!;

    private protected Entity()
       => CreationDate = DateTime.UtcNow;

    private protected void SetUpdateDateToNow()
        => UpdateDate = DateTime.UtcNow;
}
