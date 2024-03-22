namespace MyFinance.Domain.Abstractions;

public interface IUserOwnedEntity
{
    public Guid UserId { get; }
}
