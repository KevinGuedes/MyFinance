using FluentValidation;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Services.RequestValidation
{
    public class RequestValidationService : IRequestValidationService
    {
        private readonly IValidatorFactory _validatorFactory;

        public RequestValidationService(IValidatorFactory validatorFactory)
            => _validatorFactory = validatorFactory;

        public bool IsValidatableRequest(object request)
            => request is IValidatable;

        public async Task<RequestValidationResult> ValidateRequest(object request)
        {
            var validator = _validatorFactory.GetValidator(request.GetType());
            if (validator is null) return new RequestValidationResult(true);

            var context = new ValidationContext<object>(request);
            var validationResults = await validator.ValidateAsync(context);

            if (validationResults.IsValid) return new RequestValidationResult(true);
            var errors = validationResults.Errors
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

            return new RequestValidationResult(errors);
        }
    }
}
