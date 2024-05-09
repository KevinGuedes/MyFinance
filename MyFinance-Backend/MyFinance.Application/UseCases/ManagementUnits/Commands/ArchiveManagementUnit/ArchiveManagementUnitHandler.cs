using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.ArchiveManagementUnit;

internal sealed class ArchiveManagementUnitHandler(
    IManagementUnitRepository managementUnitRepository)
    : ICommandHandler<ArchiveManagementUnitCommand>
{
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;

    public async Task<Result> Handle(ArchiveManagementUnitCommand command, CancellationToken cancellationToken)
    {
        var managementUnit =
            await _managementUnitRepository.GetByIdAsync(command.Id, cancellationToken);

        if (managementUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Management Unit with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        managementUnit.Archive(command.ReasonToArchive);
        _managementUnitRepository.Update(managementUnit);

        return Result.Ok();
    }
}