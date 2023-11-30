namespace MyFinance.Application.Common.Errors;

public sealed class InternalServerError(string message = "Unexpected behavior when trying to handle request")
    : BaseError(message)
{
}
