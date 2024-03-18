using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Contracts.Common;

namespace MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

internal sealed class GetAccountTagsHandler(IAccountTagRepository accountTagRepository, ICurrentUserProvider currentUserProvider)
    : IQueryHandler<GetAccountTagsQuery, PaginatedResponse<AccountTagResponse>>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result<PaginatedResponse<AccountTagResponse>>> Handle(GetAccountTagsQuery query,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var accountTags = await _accountTagRepository.GetPaginatedAsync(
            query.PageNumber,
            query.PageSize,
            currentUserId,
            cancellationToken);

        var response = new PaginatedResponse<AccountTagResponse>(
            AccountTagMapper.DTR.Map(accountTags),
            query.PageNumber, 
            query.PageSize,
            0);

        return Result.Ok(response);
    }
}