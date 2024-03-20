using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.UnitOfWork;
using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.PipelineBehaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>(
    ILogger<UnitOfWorkBehavior<TRequest, TResponse>> logger,
    IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
    where TResponse : ResultBase
{
    private readonly ILogger<UnitOfWorkBehavior<TRequest, TResponse>> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        try
        {
            var response = await next();

            if (!_unitOfWork.HasChanges())
            {
                _logger.LogInformation("No database changes detected for {RequestName}", requestName);
                return response;
            }

            if (response.IsSuccess)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Database changes for {RequestName} successfully executed", requestName);
            }
            else
                _logger.LogWarning("Changes from {RequestName} not commited due to failure result", requestName);

            return response;
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception, "Changes not commited due to exception in {RequestName}", requestName);
            throw;
        }
    }
}