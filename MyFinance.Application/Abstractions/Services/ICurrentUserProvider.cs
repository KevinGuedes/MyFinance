using System.Security.Claims;

namespace MyFinance.Application.Abstractions.Services;

public interface ICurrentUserProvider
{
    Guid GetCurrentUserId();
    bool TryGetCurrentUserId(out Guid userId);
    bool TryGetCurrentUserId(ClaimsPrincipal claimsPrincipal, out Guid userId);
    bool TryGetCurrentUserSecurityStamp(ClaimsPrincipal claimsPrincipal, out Guid securityStamp);
}