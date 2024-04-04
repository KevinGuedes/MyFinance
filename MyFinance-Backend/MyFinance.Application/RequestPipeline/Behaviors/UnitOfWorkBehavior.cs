using FluentResults;
using MediatR;
using MyFinance.Application.Abstractions.Persistence.UnitOfWork;
using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.RequestPipeline.Behaviors;

internal sealed class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
    where TResponse : ResultBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var response = await next();

        if (response.IsSuccess)
        {
            if (_unitOfWork.HasChanges())
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
        }
        else
        {
            await transaction.RollbackAsync(cancellationToken);
        }

        return response;
    }
}
