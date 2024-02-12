using Microsoft.AspNetCore.Http;

namespace MyFinance.Application.Services.CurrentUserProvider;

public sealed class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid GetCurrentUserId()
        => Guid.Parse(GetValueByClaimType("id"));

    private string GetValueByClaimType(string claimType)
        => _httpContextAccessor.HttpContext!.User.Claims
            .Single(claim => claim.Type == claimType)
            .Value;
}
