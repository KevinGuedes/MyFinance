using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.UpdateManagementUnit;

internal sealed class UpdateManagementUnitHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<UpdateManagementUnitCommand, ManagementUnitResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<ManagementUnitResponse>> Handle(UpdateManagementUnitCommand command,
        CancellationToken cancellationToken)
    {
        var managementUnit = await _myFinanceDbContext.ManagementUnits
            .FindAsync([command.Id], cancellationToken);

        if (managementUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Management Unit with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        managementUnit.Update(command.Name, command.Description);
        _myFinanceDbContext.ManagementUnits.Update(managementUnit);

        return Result.Ok(new ManagementUnitResponse
        {
            Id = managementUnit.Id,
            Name = managementUnit.Name,
            Income = managementUnit.Income,
            Outcome = managementUnit.Outcome,
            Balance = managementUnit.Balance,
            Description = managementUnit.Description,
        });
    }
}