using FluentResults;
using MediatR.Pipeline;
using MyFinance.Application.Abstractions.Persistence.UnitOfWork;
using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.RequestPipeline.PostProcessors;

internal sealed class UnitOfWorkPostProcessor<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : IBaseCommand
    where TResponse : ResultBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        if (response.IsSuccess && _unitOfWork.HasChanges())
            await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
