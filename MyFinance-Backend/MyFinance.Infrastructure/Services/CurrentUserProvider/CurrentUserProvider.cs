using Microsoft.AspNetCore.Http;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Infrastructure.Services.CurrentUserProvider;

public sealed class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public bool IsAuthenticated
        => _httpContextAccessor.HttpContext!.User.Identity?.IsAuthenticated ?? false;

    public Guid? GetCurrentUserId()
        => IsAuthenticated ? Guid.Parse(GetValueByClaimType("id")) : default;

    private string GetValueByClaimType(string claimType)
        => _httpContextAccessor.HttpContext!.User.Claims
            .Single(claim => claim.Type == claimType)
            .Value;
}