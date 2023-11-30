using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

internal sealed class CreateAccountTagHandler(
    ILogger<CreateAccountTagHandler> logger,
    IAccountTagRepository accountTagRepository) : ICommandHandler<CreateAccountTagCommand, AccountTag>
{
    private readonly ILogger<CreateAccountTagHandler> _logger = logger;
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;

    public Task<Result<AccountTag>> Handle(CreateAccountTagCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new Account Tag Unit");
        var accountTag = new AccountTag(command.Tag, command.Description);
        _accountTagRepository.Insert(accountTag);
        _logger.LogInformation("Account Tag successfully created");

        return Task.FromResult(Result.Ok(accountTag));
    }
}
