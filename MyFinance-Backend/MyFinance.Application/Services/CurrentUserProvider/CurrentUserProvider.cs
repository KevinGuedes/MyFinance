using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MyFinance.Application.Services.CurrentUserProvider;

public sealed class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public CurrentUser GetCurrentUser()
    {
        var id = Guid.Parse(GetValueByClaimType("id"));
        var name = GetValueByClaimType(ClaimTypes.Name);
        var email = GetValueByClaimType(ClaimTypes.Email);
        return new(id, name, email);
    }

    private string GetValueByClaimType(string claimType)
        => _httpContextAccessor.HttpContext!.User.Claims
            .Single(claim => claim.Type == claimType)
            .Value;
}
