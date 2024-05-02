using Microsoft.AspNetCore.Http;
using MyFinance.Application.Abstractions.Services;
using System.Security.Claims;

namespace MyFinance.Infrastructure.Services.CurrentUserProvider;

internal sealed class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid GetCurrentUserId()
    {
        if(_httpContextAccessor.HttpContext is null)
            return default;

        return Guid.TryParse(GetValueByClaimType("id"), out var userId)
            ? userId
            : default;
    }

    private string? GetValueByClaimType(string claimType)
        => _httpContextAccessor.HttpContext!.User.FindFirstValue(claimType);
}