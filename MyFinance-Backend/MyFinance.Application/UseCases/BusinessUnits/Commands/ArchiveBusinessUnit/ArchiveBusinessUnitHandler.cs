using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;

internal sealed class ArchiveBusinessUnitHandler(IBusinessUnitRepository businessUnitRepository)
    : ICommandHandler<ArchiveBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;

    public async Task<Result> Handle(ArchiveBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        var businessUnit =
            await _businessUnitRepository.GetByIdAsync(command.Id, cancellationToken);

        if (businessUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Business Unit with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        businessUnit.Archive(command.ReasonToArchive);
        _businessUnitRepository.Update(businessUnit);

        return Result.Ok();
    }
}