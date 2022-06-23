using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Interfaces;
using MyFinance.Infra.Data.UnitOfWork;

namespace MyFinance.Application.Pipelines
{
    public class UnitOfWorkPipeline<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICommand
    {
        private readonly ILogger<UnitOfWorkPipeline<TRequest, TResponse>> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkPipeline(ILogger<UnitOfWorkPipeline<TRequest, TResponse>> logger, IUnitOfWork unitOfWork)
            => (_logger, _unitOfWork) = (logger, unitOfWork);

        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Committing database changes");
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Database changes commited");
        }
    }
}
