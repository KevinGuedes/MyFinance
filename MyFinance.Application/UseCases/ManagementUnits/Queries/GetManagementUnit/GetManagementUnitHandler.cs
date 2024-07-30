using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Common.Errors;
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
            .Where(mu => mu.Id == query.Id)
            .Select(mu => new ManagementUnitResponse
            {
                Id = mu.Id,
                Name = mu.Name,
                Description = mu.Description,
                Income = mu.Income,
                Outcome = mu.Outcome,
                Balance = mu.Balance
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (managementUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Management Unit with Id {query.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        return Result.Ok(managementUnit);
    }
}