using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Contracts.Common;

namespace MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

internal sealed class GetAccountTagsHandler(IMyFinanceDbContext myFinanceDbContext)
    : IQueryHandler<GetAccountTagsQuery, Paginated<AccountTagResponse>>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<Paginated<AccountTagResponse>>> Handle(GetAccountTagsQuery query,
        CancellationToken cancellationToken)
    {
        var totalCount = await _myFinanceDbContext.AccountTags
            .LongCountAsync(at => at.ManagementUnitId == query.ManagementUnitId, cancellationToken);

        var accountTags = await _myFinanceDbContext.AccountTags
            .AsNoTracking()
            .Where(at => at.ManagementUnitId == query.ManagementUnitId)
            .OrderBy(at => at.Tag)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(at => new AccountTagResponse
            {
                Id = at.Id,
                Tag = at.Tag,
                Description = at.Description,
            })
            .ToListAsync(cancellationToken);


        return Result.Ok(new Paginated<AccountTagResponse>
        {
            Items = accountTags.AsReadOnly(),
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        });
    }
}