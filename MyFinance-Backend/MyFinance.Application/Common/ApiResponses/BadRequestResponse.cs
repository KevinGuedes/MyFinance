using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class BadRequestResponse : BaseApiResponse<InvalidRequestError>
{
    public Dictionary<string, string[]> ValidationErrors { get; private set; }

    public BadRequestResponse(InvalidRequestError invalidRequestError)
        : base("One or more validation errors occurred", invalidRequestError) 
        => ValidationErrors = invalidRequestError.ValidationErrors;
}
