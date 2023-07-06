using MediatR;
using Microsoft.Extensions.Logging;

namespace MyFinance.Application.Pipelines
{
    public sealed class LoggingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingPipeline<TRequest, TResponse>> _logger;

        public LoggingPipeline(ILogger<LoggingPipeline<TRequest, TResponse>> logger)
            => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Name;
            _logger.LogInformation("[{RequestName}] Handling request", requestName);
            return await next();
        }
    }
}
