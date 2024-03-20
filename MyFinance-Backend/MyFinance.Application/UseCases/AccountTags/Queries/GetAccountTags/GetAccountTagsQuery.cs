using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Contracts.Common;

namespace MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

public sealed record GetAccountTagsQuery(int PageNumber, int PageSize)
    : UserBasedRequest, IQuery<Paginated<AccountTagResponse>>
{
}