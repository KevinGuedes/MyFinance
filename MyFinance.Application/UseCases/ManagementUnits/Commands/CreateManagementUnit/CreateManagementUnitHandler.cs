using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.ManagementUnit.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;

internal sealed class CreateManagementUnitHandler(IManagementUnitRepository managementUnitRepository)
    : ICommandHandler<CreateManagementUnitCommand, ManagementUnitResponse>
{
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;

    public async Task<Result<ManagementUnitResponse>> Handle(CreateManagementUnitCommand command, CancellationToken cancellationToken)
    {
        var managementUnit = new ManagementUnit(command.Name, command.Description, command.CurrentUserId);
        await _managementUnitRepository.InsertAsync(managementUnit, cancellationToken);

        return Result.Ok(ManagementUnitMapper.DTR.Map(managementUnit));
    }
}