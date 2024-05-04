namespace MyFinance.Application.Common.Errors;

public sealed class InternalServerError(string message = "MyFinance API went rogue! Sorry!")
    : BaseError(message)
{
}