using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class InternalServerErrorResponse(InternalServerError internalServerError)
    : BaseErrorResponse<InternalServerError>("MyFinance API went rogue! Sorry.", internalServerError)
{
}