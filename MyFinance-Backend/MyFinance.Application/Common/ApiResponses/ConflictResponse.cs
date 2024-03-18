using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class ConflictResponse(ConflictError conflictError)
    : BaseErrorResponse<ConflictError>("A conflict has occurred", conflictError)
{
}