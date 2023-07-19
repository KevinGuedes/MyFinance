namespace MyFinance.Application.Common.Errors;

public sealed class InvalidRequest : BaseError
{
    public Dictionary<string, string[]> ValidationErrors { get; set; }

    public InvalidRequest(string requestName, Dictionary<string, string[]> validationErrors)
        : base("Invalid request data", requestName)
        => ValidationErrors = validationErrors;
}
