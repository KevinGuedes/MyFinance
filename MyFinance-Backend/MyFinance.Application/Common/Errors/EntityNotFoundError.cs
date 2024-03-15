namespace MyFinance.Application.Common.Errors;

public class EntityNotFoundError(string message) : BaseError(message)
{
}