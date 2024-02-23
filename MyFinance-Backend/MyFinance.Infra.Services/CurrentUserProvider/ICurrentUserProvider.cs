﻿using MyFinance.Domain.Entities;

namespace MyFinance.Services.CurrentUserProvider;

public interface ICurrentUserProvider
{
    Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken);
}