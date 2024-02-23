﻿namespace MyFinance.Services.Auth;

public interface IAuthService
{
    Task SignInAsync(string userEmail);
    Task SignOutAsync();
}