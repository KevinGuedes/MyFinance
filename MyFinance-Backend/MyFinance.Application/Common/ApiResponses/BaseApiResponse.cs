using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public abstract class BaseApiResponse<TBaseError> where TBaseError : BaseError
{
    public string Title { get; private set; }
    public string ErrorMessage { get; private set; }

    protected BaseApiResponse(string title, TBaseError error)
        => (Title, ErrorMessage) = (title, error.Message);
}
