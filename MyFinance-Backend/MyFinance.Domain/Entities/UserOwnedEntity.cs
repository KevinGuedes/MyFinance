namespace MyFinance.Domain.Entities;

public abstract class UserOwnedEntity : Entity
{
    public Guid UserId { get; private set; }

    private protected UserOwnedEntity() { }

    private protected UserOwnedEntity(Guid userId)
        => UserId = userId;
}
