namespace MyFinance.Application.Services.RequestValidation
{
    public interface IRequestValidationService
    {
        bool IsValidatableRequest(object request);
        Task<RequestValidationResult> ValidateRequest(object request);
    }
}
