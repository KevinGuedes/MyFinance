namespace MyFinance.Application.Common.Errors;

public sealed class FailedToProcessRequest : BaseError
{
    public FailedToProcessRequest(string requestName)
        : base("Unexpected behavior when trying to handle request", requestName)
    {
    }
}
