namespace MyFinance.Application.Generics.Errors
{
    public class FailedToProcessRequest : BaseError
    {
        public FailedToProcessRequest(string requestName)
            : base("Unexpected behavior when trying to handle request", requestName)
        {
        }
    }
}
