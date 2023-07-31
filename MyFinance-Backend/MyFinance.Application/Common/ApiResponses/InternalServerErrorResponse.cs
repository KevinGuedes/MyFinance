using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class InternalServerErrorResponse : BaseApiResponse<InternalServerError>
{
    public InternalServerErrorResponse(InternalServerError internalServerError)
        : base("MyFinance API went rogue! Sorry.", internalServerError) { }
}
