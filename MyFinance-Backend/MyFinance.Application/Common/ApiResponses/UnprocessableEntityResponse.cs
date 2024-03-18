using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class UnprocessableEntityResponse(UnprocessableEntityError unprocessableEntityError)
    : BaseErrorResponse<UnprocessableEntityError>("Unable to process the payload data", unprocessableEntityError)
{
}