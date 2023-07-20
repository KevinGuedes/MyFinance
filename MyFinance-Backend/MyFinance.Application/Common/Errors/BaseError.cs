using FluentResults;

namespace MyFinance.Application.Common.Errors;

public abstract class BaseError : Error
{
    public BaseError(string message, string requestName) : base(message)
    {
        Metadata.Add("DateUTC", DateTime.UtcNow);
        Metadata.Add("RequestName", requestName);
    }
}
