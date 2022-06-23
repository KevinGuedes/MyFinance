using FluentValidation;

namespace MyFinance.Application.Services
{
    public class RequestValidationService 
    {
        private readonly IValidatorFactory _validatorFactory;

        public RequestValidationService(IValidatorFactory validatorFactory)
            => _validatorFactory = validatorFactory;

        public async Task<Dictionary<string, string[]>> ValidateRequest<TRequest>(TRequest request)
        {
            var validator = _validatorFactory.GetValidator<TRequest>();
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await validator.ValidateAsync(context);

            return validationResults.Errors
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

        }
    }
}
