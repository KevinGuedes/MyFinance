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
        var totalCount = await _accountTagRepository.GetTotalCountAsync(
            query.ManagementUnitId,
            cancellationToken);

        var accountTags = await _accountTagRepository.GetPaginatedAsync(
            query.ManagementUnitId,
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        var response = AccountTagMapper.DTR.Map(
            accountTags,
            query.PageNumber,
            query.PageSize,
            totalCount);

        return Result.Ok(response);
    }
}