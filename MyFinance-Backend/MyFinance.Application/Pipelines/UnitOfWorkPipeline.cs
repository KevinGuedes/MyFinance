using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Infra.Data.UnitOfWork;

namespace MyFinance.Application.Pipelines;

public sealed class UnitOfWorkPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICommand<TResponse>
    where TResponse : ResultBase
{
    private readonly ILogger<UnitOfWorkPipeline<TRequest, TResponse>> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkPipeline(ILogger<UnitOfWorkPipeline<TRequest, TResponse>> logger, IUnitOfWork unitOfWork)
        => (_logger, _unitOfWork) = (logger, unitOfWork);

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request.GetType();

        try
        {
            await _unitOfWork.BeginTrasactionAsync(cancellationToken);
            _logger.LogInformation("[{RequestName}] Listening to database changes", requestName);
            var response = await next();

            if (response.IsSuccess)
            {
                _logger.LogInformation("[{RequestName}] Committing database changes", requestName);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                _logger.LogInformation("[{RequestName}] Database changes successfully commited", requestName);
            }
            else
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogWarning("[{RequestName}] Changes not commited due to failure response", requestName);
            }

            return response;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogWarning("[{RequestName}] Changes not commited due to exception throwed", requestName);
            throw;
        }
    }
}
