namespace MyFinance.Application.Common.Errors;

public sealed class InvalidRequestError : BaseError
{
    public Dictionary<string, string[]> ValidationErrors { get; set; }

    public InvalidRequestError(Dictionary<string, string[]> validationErrors)
        : base("Invalid payload data, check validation errors for more details")
        => ValidationErrors = validationErrors;
}
