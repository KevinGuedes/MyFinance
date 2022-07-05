using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Generics.Errors;

namespace MyFinance.Application.Pipelines
{
    public sealed class ExceptionHandlerPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResultBase, new()
    {
        private readonly ILogger<ExceptionHandlerPipeline<TRequest, TResponse>> _logger;

        public ExceptionHandlerPipeline(ILogger<ExceptionHandlerPipeline<TRequest, TResponse>> logger)
            => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception exception)
            {
                var requestName = request.GetType();
                _logger.LogError(exception, "[{RequestName}] Failed to handle request", requestName);

                var error = Result.Fail(new FailedToProcessRequest(requestName.ToString()).CausedBy(exception));
                var response = new TResponse();
                response.Reasons.AddRange(error.Reasons);

                return response;
            }
        }
    }
}
