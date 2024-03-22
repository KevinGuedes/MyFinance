using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.PipelineBehaviors;

internal sealed class RequestValidationBehavior<TRequest, TResponse>(
    ILogger<RequestValidationBehavior<TRequest, TResponse>> logger,
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseAppRequest
    where TResponse : ResultBase, new()
{
    private readonly ILogger<RequestValidationBehavior<TRequest, TResponse>> _logger = logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        if (_validators.Count() is 0)
        {
            _logger.LogWarning("No validators found for {RequestName}", requestName);
            return await next();
        }

        var validationContext = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(validators =>
            validators.ValidateAsync(validationContext, cancellationToken)));
        var validationErrors = validationResults
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationResult => validationResult is not null)
            .ToList();

        //var errors = validationResults
        //    .SelectMany(validationResult => validationResult.Errors)
        //    .Where(validationResult => validationResult is not null)
        //    .GroupBy(
        //        validationResult => validationResult.PropertyName,
        //        validationResult => validationResult.ErrorMessage,
        //        (propertyName, errorMessages) => new
        //        {
        //            Key = string.Concat(char.ToLower(propertyName[0]), propertyName[1..]),
        //            Values = errorMessages.Distinct().ToArray()
        //        })
        //    .ToDictionary(dictionaryData => dictionaryData.Key, dictionaryData => dictionaryData.Values);

        if (validationErrors.Count is 0) return await next();

        var response = new TResponse();
        var failedResult = Result.Fail(new InvalidRequestError(validationErrors));
        response.Reasons.AddRange(failedResult.Reasons);
        return response;
    }
}