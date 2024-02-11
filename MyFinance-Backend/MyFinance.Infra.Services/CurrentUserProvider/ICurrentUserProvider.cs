using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Services.CurrentUserProvider;

public interface ICurrentUserProvider
{
    Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken);
}
