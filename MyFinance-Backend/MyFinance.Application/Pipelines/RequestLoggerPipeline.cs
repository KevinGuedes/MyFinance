using MediatR;
using Microsoft.Extensions.Logging;

namespace MyFinance.Application.Pipelines
{
    public class RequestLoggerPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<RequestLoggerPipeline<TRequest, TResponse>> _logger;

        public RequestLoggerPipeline(ILogger<RequestLoggerPipeline<TRequest, TResponse>> logger)
            => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType().Name;
            _logger.LogInformation("Handling {RequestName}", requestName);
            var result = await next();
            _logger.LogInformation("{RequestName} successfully handled", requestName);
            return result;
        }
    }
}
