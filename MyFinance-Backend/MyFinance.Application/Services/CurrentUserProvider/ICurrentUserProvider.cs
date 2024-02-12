namespace MyFinance.Application.Services.CurrentUserProvider;

public interface ICurrentUserProvider
{
    Guid GetCurrentUserId();
}
