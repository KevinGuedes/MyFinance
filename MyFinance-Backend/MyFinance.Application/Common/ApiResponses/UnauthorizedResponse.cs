using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;
public sealed class UnauthorizedResponse(UnauthorizedError unauthorizedError) 
    : BaseApiResponse<UnauthorizedError>("User unauthorized", unauthorizedError)
{
}
