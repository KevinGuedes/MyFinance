using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

internal sealed class CreateAccountTagHandler(
    IAccountTagRepository accountTagRepository,
    IManagementUnitRepository managementUnitRepository)
    : ICommandHandler<CreateAccountTagCommand, AccountTagResponse>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;

    public async Task<Result<AccountTagResponse>> Handle(CreateAccountTagCommand command, CancellationToken cancellationToken)
    {
        var managementUnit = await _managementUnitRepository.GetByIdAsync(command.ManagementUnitId, cancellationToken);
        if (managementUnit is null)
        {
            var errorMessage = $"Management Unit with Id {command.ManagementUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var accountTag = new AccountTag(
            managementUnit,
            command.Tag,
            command.Description,
            command.CurrentUserId);
        await _accountTagRepository.InsertAsync(accountTag, cancellationToken);

        return Result.Ok(AccountTagMapper.DTR.Map(accountTag));
    }
}