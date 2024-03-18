using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

internal sealed class CreateAccountTagHandler(
    ILogger<CreateAccountTagHandler> logger,
    IAccountTagRepository accountTagRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<CreateAccountTagCommand, AccountTagResponse>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    private readonly ILogger<CreateAccountTagHandler> _logger = logger;

    public Task<Result<AccountTagResponse>> Handle(CreateAccountTagCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var accountTag = new AccountTag(command.Tag, command.Description, currentUserId);
        _accountTagRepository.Insert(accountTag);

        var result = Result.Ok(AccountTagMapper.DTR.Map(accountTag));
        return Task.FromResult(result);
    }
}