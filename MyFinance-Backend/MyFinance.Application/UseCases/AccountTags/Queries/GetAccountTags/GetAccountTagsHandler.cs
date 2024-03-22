using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Contracts.Common;

namespace MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

internal sealed class GetAccountTagsHandler(IAccountTagRepository accountTagRepository)
    : IQueryHandler<GetAccountTagsQuery, Paginated<AccountTagResponse>>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;

    public async Task<Result<Paginated<AccountTagResponse>>> Handle(GetAccountTagsQuery query,
        CancellationToken cancellationToken)
    {
        var accountTags = await _accountTagRepository.GetPaginatedAsync(
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        var response = new Paginated<AccountTagResponse>(
            AccountTagMapper.DTR.Map(accountTags),
            query.PageNumber,
            query.PageSize,
            0);

        return Result.Ok(response);
    }
}