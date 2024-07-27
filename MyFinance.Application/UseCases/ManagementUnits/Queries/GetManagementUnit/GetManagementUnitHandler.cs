using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnit;

internal sealed class GetManagementUnitHandler(IMyFinanceDbContext myFinanceDbContext)
    : IQueryHandler<GetManagementUnitQuery, ManagementUnitResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<ManagementUnitResponse>> Handle(GetManagementUnitQuery query,
        CancellationToken cancellationToken)
    {
        var managementUnit = await _myFinanceDbContext.ManagementUnits
            .FindAsync([query.Id], cancellationToken);

        if (managementUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Management Unit with Id {query.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        return Result.Ok(new ManagementUnitResponse 
        {
            Id = managementUnit.Id,
            Name = managementUnit.Name,
            Description = managementUnit.Description,
            Income = managementUnit.Income,
            Outcome = managementUnit.Outcome,
            Balance = managementUnit.Balance,
        });
   }
}