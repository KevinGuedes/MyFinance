namespace MyFinance.Application.Generics.Errors
{
    public class FailedToProcessRequest : BaseError
    {
        public FailedToProcessRequest(string requestName)
            : base("Failed to process request due to unexpected behavior")
        {
            Metadata.Add("RequestName", requestName);
        }
    }
}
