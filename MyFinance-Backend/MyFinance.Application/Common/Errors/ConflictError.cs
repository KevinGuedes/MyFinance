namespace MyFinance.Application.Common.Errors;

public sealed class ConflictError(string message) : BaseError(message)
{
}
