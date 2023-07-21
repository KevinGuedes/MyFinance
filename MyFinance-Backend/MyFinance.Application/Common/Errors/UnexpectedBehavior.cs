namespace MyFinance.Application.Common.Errors;

public sealed class UnexpectedBehavior : BaseError
{
    public UnexpectedBehavior(string requestName)
        : base("Unexpected behavior when trying to handle request", requestName)
    {
    }
}
