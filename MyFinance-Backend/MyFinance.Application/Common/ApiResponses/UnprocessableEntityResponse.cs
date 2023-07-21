using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class UnprocessableEntityResponse : BaseApiResponse<UnprocessableEntityError>
{
    public UnprocessableEntityResponse(UnprocessableEntityError unprocessableEntityError) 
        : base("Unable to process entity data", unprocessableEntityError)
    {
    }
}
