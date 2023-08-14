using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

public sealed record GetAccountTagsQuery(int Page, int PageSize) : IQuery<IEnumerable<AccountTag>>
{
}

