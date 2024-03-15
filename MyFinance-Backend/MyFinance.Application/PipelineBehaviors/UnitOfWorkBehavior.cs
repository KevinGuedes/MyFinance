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

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        try
        {
            _logger.LogInformation("[{RequestName}] Listening to database changes", requestName);

            var response = await next();

            if (!_unitOfWork.HasChanges())
            {
                _logger.LogInformation("[{RequestName}] No database changes detected", requestName);
                return response;
            }

            if (response.IsSuccess)
            {
                _logger.LogInformation("[{RequestName}] Saving database changes", requestName);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("[{RequestName}] Database changes successfully saved", requestName);
            }
            else
                _logger.LogWarning("[{RequestName}] Changes not commited due to failure response", requestName);

            return response;
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception, "[{RequestName}] Changes not commited due to exception", requestName);
            throw;
        }
    }
}
