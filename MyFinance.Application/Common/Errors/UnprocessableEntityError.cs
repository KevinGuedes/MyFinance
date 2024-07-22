namespace MyFinance.Application.Common.Errors;

public sealed class UnprocessableEntityError(string message) : BaseError(message)
{
}