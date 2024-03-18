using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class UnauthorizedResponse(UnauthorizedError unauthorizedError)
    : BaseErrorResponse<UnauthorizedError>("User unauthorized", unauthorizedError)
{
}