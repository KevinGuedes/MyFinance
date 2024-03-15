namespace MyFinance.Application.Abstractions.Services;

public interface ICurrentUserProvider
{
    Guid GetCurrentUserId();
}
