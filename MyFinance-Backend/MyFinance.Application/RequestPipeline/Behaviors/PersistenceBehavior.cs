using FluentResults;
using MediatR;
using MyFinance.Application.Abstractions.Persistence.UnitOfWork;
using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.RequestPipeline.Behaviors;

internal sealed class PersistenceBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAutomaticallyPersisted
    where TResponse : ResultBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return response;
    }
}