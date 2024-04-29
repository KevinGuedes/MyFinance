﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;
using System.Security.Claims;

namespace MyFinance.Infrastructure.Services.SignInManager;

internal sealed class SignInManager(IHttpContextAccessor httpContextAccessor)
    : ISignInManager
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task SignInAsync(User user)
    {
        var claims = new List<Claim>
        {
            new("id", user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties { IsPersistent = true };

        await _httpContextAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties);
    }

    public Task SignOutAsync()
        => _httpContextAccessor.HttpContext!
            .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
}
