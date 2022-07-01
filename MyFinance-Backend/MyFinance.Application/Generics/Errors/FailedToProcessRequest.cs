namespace MyFinance.Application.Generics.Errors
{
    public class FailedToProcessRequest : BaseError
    {
        public FailedToProcessRequest(string requestName)
            : base("Unexpected when handling request", requestName)
        {
        }
    }
}
