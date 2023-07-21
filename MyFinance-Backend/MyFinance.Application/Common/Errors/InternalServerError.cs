namespace MyFinance.Application.Common.Errors;

public sealed class InternalServerError : BaseError
{
    public InternalServerError(string message = "Unexpected behavior when trying to handle request") 
        : base(message)
    {
    }
}
