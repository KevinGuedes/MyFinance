﻿using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

internal sealed class CreateAccountTagHandler(
    ILogger<CreateAccountTagHandler> logger,
    IAccountTagRepository accountTagRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<CreateAccountTagCommand, AccountTag>
{
    private readonly ILogger<CreateAccountTagHandler> _logger = logger;
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public Task<Result<AccountTag>> Handle(CreateAccountTagCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        _logger.LogInformation("Creating new Account Tag Unit");
        var accountTag = new AccountTag(command.Tag, command.Description, currentUserId);
        _accountTagRepository.Insert(accountTag);
        _logger.LogInformation("Account Tag successfully created");

        return Task.FromResult(Result.Ok(accountTag));
    }
}
