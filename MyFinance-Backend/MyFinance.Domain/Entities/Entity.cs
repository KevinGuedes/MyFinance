namespace MyFinance.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime? UpdateDate { get; private set; }

    private protected Entity()
       => (Id, CreationDate) = (Guid.NewGuid(), DateTime.UtcNow);

    private protected void SetUpdateDateToNow()
        => UpdateDate = DateTime.UtcNow;
}
