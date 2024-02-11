using Microsoft.AspNetCore.Http;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using System.Security.Claims;

namespace MyFinance.Infra.Services.CurrentUserProvider;

public sealed class CurrentUserProvider(
    IHttpContextAccessor httpContextAccessor, 
    IUserRepository userRepository) : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IUserRepository _userRepository = userRepository;

    public Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var email = GetValueByClaimType(ClaimTypes.Email);
        return _userRepository.GetByEmailAsync(email, cancellationToken);
    }

    private string GetValueByClaimType(string claimType) 
        => _httpContextAccessor.HttpContext!.User.Claims
            .Single(claim => claim.Type == claimType)
            .Value;
}
