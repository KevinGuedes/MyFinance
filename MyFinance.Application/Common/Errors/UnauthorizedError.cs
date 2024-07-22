namespace MyFinance.Application.Common.Errors;

public sealed class UnauthorizedError(string message) : BaseError(message)
{
}