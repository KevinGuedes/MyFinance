using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

public class UnarchiveBusinessUnitHandler(IBusinessUnitRepository businessUnitRepository) 
    : ICommandHandler<UnarchiveBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;

    public async Task<Result> Handle(UnarchiveBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        var businessUnit = await _businessUnitRepository.GetByIdAsync(command.Id, command.CurrentUserId, cancellationToken);

        if (businessUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Business Unit with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        businessUnit.Unarchive();
        _businessUnitRepository.Update(businessUnit);

        return Result.Ok();
    }
}