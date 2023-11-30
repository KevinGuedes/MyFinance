namespace MyFinance.Application.Common.Errors;

public sealed class InvalidRequestError(Dictionary<string, string[]> validationErrors)
    : BaseError("Invalid payload data, check validation errors for more details")
{
    public Dictionary<string, string[]> ValidationErrors { get; set; } = validationErrors;
}
