using Microsoft.AspNetCore.Http;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Infrastructure.Common;
using System.Security.Claims;

namespace MyFinance.Infrastructure.Services.CurrentUserProvider;

internal sealed class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid GetCurrentUserId()
    {
        if (_httpContextAccessor.HttpContext is null)
            return default;

        return Guid.TryParse(GetValueByClaimType(CustomClaimTypes.Id), out var userId)
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

        return Guid.TryParse(GetValueByClaimType(CustomClaimTypes.Id), out userId);
    }

    public bool TryGetCurrentUserId(ClaimsPrincipal claimsPrincipal, out Guid userId)
        => Guid.TryParse(GetValueByClaimType(claimsPrincipal, CustomClaimTypes.Id), out userId);
    
    public bool TryGetCurrentUserSecurityStamp(ClaimsPrincipal claimsPrincipal, out Guid securityStamp)
        => Guid.TryParse(GetValueByClaimType(claimsPrincipal, CustomClaimTypes.SecurityStamp), out securityStamp);

    private static string? GetValueByClaimType(ClaimsPrincipal claimsPrincipal, string claimType)
        => claimsPrincipal.FindFirstValue(claimType);

    private string? GetValueByClaimType(string claimType)
        => _httpContextAccessor.HttpContext!.User.FindFirstValue(claimType);
}