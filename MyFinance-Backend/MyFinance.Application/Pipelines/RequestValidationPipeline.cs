using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Generics.Errors;

namespace MyFinance.Application.Pipelines
{
    public class RequestValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResultBase, new()
    {
        private readonly ILogger<RequestValidationPipeline<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationPipeline(
            ILogger<RequestValidationPipeline<TRequest, TResponse>> logger,
            IEnumerable<IValidator<TRequest>> validators)
            => (_logger, _validators) = (logger, validators);

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType().Name;

            if (!_validators.Any())
            {
                _logger.LogWarning("[{RequestName}] No validators found", requestName);
                return await next();
            }

            _logger.LogInformation("[{RequestName}] Validating request data", requestName);
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(validators => validators.ValidateAsync(context, cancellationToken)));
            var errors = validationResults
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationResult => validationResult is not null)
                .ToList()
                .GroupBy(
                    validationResult => validationResult.PropertyName,
                    validationResult => validationResult.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                .ToDictionary(dictionaryData => dictionaryData.Key, dictionaryData => dictionaryData.Values);

            if (errors.Any())
            {
                _logger.LogError("[{RequestName}] Invalid request data", requestName);
                var response = new TResponse();
                var error = Result.Fail(new InvalidRequestData(requestName, errors));
                response.Reasons.AddRange(error.Reasons);
                return response;
            }

            return await next();
        }
    }
}
