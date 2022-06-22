namespace MyFinance.Application.Exceptions
{
    public class InvalidRequestException : BaseException
    {
        public InvalidRequestException(IReadOnlyDictionary<string, string[]> errors)
            : base("One or more validation errors occurred", "Invalid data for request handler")
            => Errors = errors;

        public IReadOnlyDictionary<string, string[]> Errors { get; private set; }
    }
}
