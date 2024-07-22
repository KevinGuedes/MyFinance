using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.UpdateManagementUnit;

internal sealed class UpdateManagementUnitHandler(IManagementUnitRepository managementUnitRepository)
    : ICommandHandler<UpdateManagementUnitCommand, ManagementUnitResponse>
{
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;

    public async Task<Result<ManagementUnitResponse>> Handle(UpdateManagementUnitCommand command,
        CancellationToken cancellationToken)
    {
        var managementUnit = await _managementUnitRepository.GetByIdAsync(command.Id, cancellationToken);

        if (managementUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Management Unit with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        managementUnit.Update(command.Name, command.Description);
        _managementUnitRepository.Update(managementUnit);

        return Result.Ok(ManagementUnitMapper.DTR.Map(managementUnit));
    }
}