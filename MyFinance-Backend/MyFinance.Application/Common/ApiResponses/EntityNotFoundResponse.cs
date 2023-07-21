using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class EntityNotFoundResponse : BaseApiResponse<EntityNotFoundError>
{
    public EntityNotFoundResponse(EntityNotFoundError entityNotFoundError) 
        : base("Entity not found", entityNotFoundError)
    {
    }
}
