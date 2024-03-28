using MyFinance.Domain.Common;

namespace MyFinance.Application.Common.Errors;

public class EntityNotFoundError(string message) : BaseError(message)
{
}

public class EntityNotFoundError<TEntity>(Guid id) 
    : BaseError($"{nameof(TEntity)} with Id {id} not found")
    where TEntity : Entity
{
}