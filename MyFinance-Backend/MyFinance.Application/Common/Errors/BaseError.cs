using FluentResults;

namespace MyFinance.Application.Common.Errors;

public abstract class BaseError : Error
{
    public BaseError(string message) : base(message)
        => Metadata.Add("DateUTC", DateTime.UtcNow);
}