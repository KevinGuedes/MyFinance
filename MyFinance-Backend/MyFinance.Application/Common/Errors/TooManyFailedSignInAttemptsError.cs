namespace MyFinance.Application.Common.Errors;

public sealed class TooManyFailedSignInAttemptsError(DateTime lockoutEndOnUtc) 
    : BaseError($"Maximum Sign In attempts reached. Retry after {lockoutEndOnUtc:R}")
{
    public DateTime LockoutEndOnUtc { get; } = lockoutEndOnUtc;
}
