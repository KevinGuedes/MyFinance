using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class EntityNotFoundResponse(EntityNotFoundError entityNotFoundError) 
    : BaseApiResponse<EntityNotFoundError>("Entity not found", entityNotFoundError)
{
}
