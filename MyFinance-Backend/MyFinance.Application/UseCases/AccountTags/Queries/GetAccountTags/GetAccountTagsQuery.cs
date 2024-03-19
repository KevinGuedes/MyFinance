using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Contracts.Common;

namespace MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

public sealed record GetAccountTagsQuery(int PageNumber, int PageSize) 
    : IQuery<Paginated<AccountTagResponse>>, IUserBasedRequest
{
    public Guid CurrentUserId { get; set; }
}