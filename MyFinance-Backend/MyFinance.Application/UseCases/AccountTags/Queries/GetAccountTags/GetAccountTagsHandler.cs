using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

internal sealed class GetAccountTagsHandler(
    ILogger<GetAccountTagsHandler> logger,
    IAccountTagRepository accountTagRepository,
    ICurrentUserProvider currentUserProvider) : IQueryHandler<GetAccountTagsQuery, IEnumerable<AccountTag>>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    private readonly ILogger<GetAccountTagsHandler> _logger = logger;

    public async Task<Result<IEnumerable<AccountTag>>> Handle(GetAccountTagsQuery query,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        _logger.LogInformation("Retrieving Account Tags from database");
        var accountTags = await _accountTagRepository.GetPaginatedAsync(
            query.Page,
            query.PageSize,
            currentUserId,
            cancellationToken);
        _logger.LogInformation("Account Tags successfully retrived from database");

        return Result.Ok(accountTags);
    }
}