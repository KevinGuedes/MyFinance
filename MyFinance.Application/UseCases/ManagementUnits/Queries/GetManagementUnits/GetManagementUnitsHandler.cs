using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnits;

internal sealed class GetManagementUnitsHandler(IMyFinanceDbContext myFinanceDbContext)
    : IQueryHandler<GetManagementUnitsQuery, Paginated<ManagementUnitResponse>>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<Paginated<ManagementUnitResponse>>> Handle(GetManagementUnitsQuery query,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var totalCount = await _myFinanceDbContext.ManagementUnits
                .LongCountAsync(cancellationToken);

            var managementUnits = await _myFinanceDbContext.ManagementUnits
                .AsNoTracking()
                .OrderBy(mu => mu.Name)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(mu => new ManagementUnitResponse
                {
                    Id = mu.Id,
                    Name = mu.Name,
                    Description = mu.Description,
                    Income = mu.Income,
                    Outcome = mu.Outcome
                })
                .ToListAsync(cancellationToken);

            return Result.Ok(new Paginated<ManagementUnitResponse>
            {
                Items = managementUnits.AsReadOnly(),
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount
            });
        }
        else
        {
            var totalCount = await _myFinanceDbContext.ManagementUnits
                .Where(mu => mu.Name.Contains(query.SearchTerm))
                .LongCountAsync(cancellationToken);

            var managementUnits = await _myFinanceDbContext.ManagementUnits
                .AsNoTracking()
                .Where(mu => mu.Name.Contains(query.SearchTerm))
                .OrderBy(mu => mu.Name)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(mu => new ManagementUnitResponse
                {
                    Id = mu.Id,
                    Name = mu.Name,
                    Description = mu.Description,
                    Income = mu.Income,
                    Outcome = mu.Outcome,
                })
                .ToListAsync(cancellationToken);

            return Result.Ok(new Paginated<ManagementUnitResponse>
            {
                Items = managementUnits.AsReadOnly(),
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount
            });
        }
    }
}