using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Interfaces;
using MyFinance.Infra.Data.UnitOfWork;

namespace MyFinance.Application.Pipelines
{
    public sealed class UnitOfWorkPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICommand<TResponse>
        where TResponse : ResultBase
    {
        private readonly ILogger<UnitOfWorkPipeline<TRequest, TResponse>> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkPipeline(ILogger<UnitOfWorkPipeline<TRequest, TResponse>> logger, IUnitOfWork unitOfWork)
            => (_logger, _unitOfWork) = (logger, unitOfWork);

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType();

            _logger.LogInformation("[{RequestName}] Listening to database changes", requestName);
            var response = await next();

            if (response.IsSuccess)
            {
                _logger.LogInformation("[{RequestName}] Committing database changes", requestName);
                await _unitOfWork.CommitAsync(cancellationToken);
                _logger.LogInformation("[{RequestName}] Database changes commited", requestName);
            }
            else
                _logger.LogWarning("[{RequestName}] Changes not commited due to failure response", requestName);

            return response;
        }
    }
}
