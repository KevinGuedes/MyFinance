using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class BadRequestResponse(InvalidRequestError invalidRequestError)
    : BaseApiResponse<InvalidRequestError>("One or more validation errors occurred", invalidRequestError)
{
    public Dictionary<string, string[]> ValidationErrors { get; private set; } = invalidRequestError.ValidationErrors;
}