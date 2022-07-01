using FluentResults;

namespace MyFinance.Application.Generics.Errors
{
    public class BaseError : Error
    {
        public BaseError(string message) : base(message)
        {
            Metadata.Add("DateUTC", DateTime.UtcNow);
        }
    }
}
