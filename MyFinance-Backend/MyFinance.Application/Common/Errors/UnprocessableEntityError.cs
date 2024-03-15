namespace MyFinance.Application.Common.Errors;

public class UnprocessableEntityError(string message) : BaseError(message)
{
}