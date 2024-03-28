using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;

internal sealed class RegisterTransferHandler(
    IBusinessUnitRepository businessUnitRepository,
    ITransferRepository transferRepository,
    IAccountTagRepository accountTagRepository,
    ICategoryRepository categoryRepository) : ICommandHandler<RegisterTransferCommand, TransferResponse>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<Result<TransferResponse>> Handle(RegisterTransferCommand command,
        CancellationToken cancellationToken)
    {
        var businessUnit = await _businessUnitRepository.GetByIdAsync(command.BusinessUnitId, cancellationToken);
        if (businessUnit is null)
        {
            var errorMessage = $"Business Unit with Id {command.BusinessUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var accountTag = await _accountTagRepository.GetByIdAsync(command.AccountTagId, cancellationToken);
        if (accountTag is null)
        {
            var errorMessage = $"Account Tag with Id {command.AccountTagId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);
        if (category is null)
        {
            var errorMessage = $"Category with Id {command.AccountTagId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var transfer = new Transfer(
            command.Value,
            command.RelatedTo,
            command.Description,
            command.SettlementDate,
            command.Type,
            command.CurrentUserId,
            businessUnit,
            accountTag,
            category);

        businessUnit.RegisterValue(transfer.Value, transfer.Type);

        _businessUnitRepository.Update(businessUnit);
        _transferRepository.Insert(transfer);

        return Result.Ok(TransferMapper.DTR.Map(transfer));
    }
}