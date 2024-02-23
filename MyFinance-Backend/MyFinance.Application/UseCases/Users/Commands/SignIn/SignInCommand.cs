﻿using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.SignIn;

public sealed record SignInCommand(string Email, string PlainTextPassword) : ICommand
{
}