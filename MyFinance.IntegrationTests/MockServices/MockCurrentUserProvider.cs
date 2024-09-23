using MyFinance.Application.Abstractions.Services;
using System.Security.Claims;

namespace MyFinance.IntegrationTests.MockServices;

internal class MockCurrentUserProvider(Guid mockUserId) : ICurrentUserProvider
{
    public Guid GetCurrentUserId() => mockUserId;

    public bool TryGetCurrentUserId(out Guid userId)
    {
        userId = mockUserId;
        return true;
    }

    public bool TryGetCurrentUserId(ClaimsPrincipal claimsPrincipal, out Guid userId)
    {
        userId = mockUserId;
        return true;
    }

    public bool TryGetCurrentUserSecurityStamp(ClaimsPrincipal claimsPrincipal, out Guid securityStamp)
    {
        securityStamp = Guid.NewGuid();
        return true;
    }
}