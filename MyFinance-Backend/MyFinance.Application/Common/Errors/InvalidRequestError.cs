using FluentValidation.Results;

namespace MyFinance.Application.Common.Errors;

public sealed class InvalidRequestError(IEnumerable<ValidationFailure> validationErrors)
    : BaseError("Invalid payload data, check validation errors for more details")
{
    public IEnumerable<ValidationFailure> ValidationErrors { get; init; } = validationErrors;
}