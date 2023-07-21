namespace MyFinance.Application.Common.Errors;

public class UnprocessableEntityError : BaseError
{
    public UnprocessableEntityError(string message) : base(message)
    {
    }
}
