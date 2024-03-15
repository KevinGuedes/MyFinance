namespace MyFinance.Domain.Entities;

public abstract class UserOwnedEntity : Entity
{
    private protected UserOwnedEntity()
    {
    }

    private protected UserOwnedEntity(Guid userId)
        => UserId = userId;

    public Guid UserId { get; private set; }
}