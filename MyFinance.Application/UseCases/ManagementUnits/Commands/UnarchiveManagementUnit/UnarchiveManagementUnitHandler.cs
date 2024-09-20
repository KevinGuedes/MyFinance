using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.UnarchiveManagementUnit;

public sealed class UnarchiveManagementUnitHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<UnarchiveManagementUnitCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result> Handle(UnarchiveManagementUnitCommand command, CancellationToken cancellationToken)
    {
        var managementUnit = await _myFinanceDbContext.ManagementUnits
            .FindAsync([command.Id], cancellationToken);

        if (managementUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Management Unit with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        managementUnit.Unarchive();
        _myFinanceDbContext.ManagementUnits.Update(managementUnit);

        return Result.Ok();
    }
}