namespace MyFinance.Application.Common.Errors;

public class EntityNotFoundError : BaseError
{
    public EntityNotFoundError(string message) : base(message)
    {
    }
}
