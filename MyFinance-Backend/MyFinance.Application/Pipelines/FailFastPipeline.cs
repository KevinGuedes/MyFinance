using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Exceptions;

namespace MyFinance.Application.Pipelines
{
    public class FailFastPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<FailFastPipeline<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public FailFastPipeline(
            ILogger<FailFastPipeline<TRequest, TResponse>> logger,
            IEnumerable<IValidator<TRequest>> validators)
            => (_logger, _validators) = (logger, validators);

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType().Name;

            if (!_validators.Any())
            {
                _logger.LogInformation("No validators found for {RequestName}", requestName);
                return await next();
            }

            _logger.LogInformation("Validating {RequestName} data", requestName);

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
                _logger.LogError("Invalid {RequestName} data", requestName);
                throw new InvalidRequestException(errors);
            }

            return await next();
        }
    }
}
