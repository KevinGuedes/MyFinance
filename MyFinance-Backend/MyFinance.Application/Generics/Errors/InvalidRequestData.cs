namespace MyFinance.Application.Generics.Errors
{
    public class InvalidRequestData : BaseError
    {
        public Dictionary<string, string[]> ValidationErrors { get; set; }

        public InvalidRequestData(string requestName, Dictionary<string, string[]> validationErrors)
            : base("Invalid request data")
        {
            ValidationErrors = validationErrors;
            Metadata.Add("RequestName", requestName);
        }
    }
}
