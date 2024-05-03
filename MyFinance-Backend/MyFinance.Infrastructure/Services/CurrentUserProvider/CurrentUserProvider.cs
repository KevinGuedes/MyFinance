using Microsoft.AspNetCore.Http;
using MyFinance.Application.Abstractions.Services;
using System.Security.Claims;

namespace MyFinance.Infrastructure.Services.CurrentUserProvider;

internal sealed class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid GetCurrentUserId()
    {
        if (_httpContextAccessor.HttpContext is null)
            return default;

        return Guid.TryParse(GetValueByClaimType("id"), out var userId)
            ? userId
            : default;
    }

    public bool TryGetCurrentUserId(out Guid userId)
    {
        if(_httpContextAccessor.HttpContext is null)
        {
            userId = default;
            return false;
        }

        return Guid.TryParse(GetValueByClaimType("id"), out userId);
    }

    public bool TryGetCurrentUserSecurityStamp(ClaimsPrincipal claimsPrincipal, out Guid securityStamp)
        => Guid.TryParse(GetValueByClaimType(claimsPrincipal, "security-stamp"), out securityStamp);

    public bool TryGetCurrentUserId(ClaimsPrincipal claimsPrincipal, out Guid userId)
        => Guid.TryParse(GetValueByClaimType(claimsPrincipal, "id"), out userId);
    
    private static string? GetValueByClaimType(ClaimsPrincipal claimsPrincipal, string claimType)
        => claimsPrincipal.FindFirstValue(claimType);

    private string? GetValueByClaimType(string claimType)
        => _httpContextAccessor.HttpContext!.User.FindFirstValue(claimType);
}