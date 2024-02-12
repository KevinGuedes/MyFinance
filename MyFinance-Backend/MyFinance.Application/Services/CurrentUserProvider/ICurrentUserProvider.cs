using MyFinance.Domain.Entities;

namespace MyFinance.Application.Services.CurrentUserProvider;

public interface ICurrentUserProvider
{
    Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken);
}
