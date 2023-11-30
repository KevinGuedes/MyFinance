using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class InternalServerErrorResponse(InternalServerError internalServerError)
    : BaseApiResponse<InternalServerError>("MyFinance API went rogue! Sorry.", internalServerError)
{
}
