using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Contracts.Category.Responses;
using MyFinance.Contracts.Common;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Categories.Queries.GetCategories;

internal sealed class GetCategoriesHandler(IMyFinanceDbContext myFinanceDbContext)
    : IQueryHandler<GetCategoriesQuery, Paginated<CategoryResponse>>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<Paginated<CategoryResponse>>> Handle(GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var totalCount = await _myFinanceDbContext.Categories
            .LongCountAsync(category => category.ManagementUnitId == query.ManagementUnitId, cancellationToken);

        var categories = await _myFinanceDbContext.Categories
            .AsNoTracking()
            .Where(category => category.ManagementUnitId == query.ManagementUnitId)
            .OrderBy(category => category.Name)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(at => new CategoryResponse
            {
                Id = at.Id,
                Name = at.Name
            })
            .ToListAsync(cancellationToken);

        return Result.Ok(new Paginated<CategoryResponse>
        {
            Items = categories.AsReadOnly(),
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        });
    }
}