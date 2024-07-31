using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.ManagementUnit.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;

internal sealed class CreateManagementUnitHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<CreateManagementUnitCommand, ManagementUnitResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<ManagementUnitResponse>> Handle(CreateManagementUnitCommand command, CancellationToken cancellationToken)
    {
        var managementUnit = new ManagementUnit(command.Name, command.Description, command.CurrentUserId);
        await _myFinanceDbContext.ManagementUnits.AddAsync(managementUnit, cancellationToken);

        return Result.Ok(new ManagementUnitResponse
        {
            Id = managementUnit.Id,
            Name = managementUnit.Name,
            Income = managementUnit.Income,
            Outcome = managementUnit.Outcome,
            Description = managementUnit.Description,
        });
    }
}