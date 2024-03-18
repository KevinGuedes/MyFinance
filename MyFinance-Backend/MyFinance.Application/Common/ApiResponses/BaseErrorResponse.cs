using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public abstract class BaseErrorResponse<TBaseError>(string title, TBaseError error) where TBaseError : BaseError
{
    public string Title { get; private set; } = title;
    public string ErrorMessage { get; private set; } = error.Message;
}