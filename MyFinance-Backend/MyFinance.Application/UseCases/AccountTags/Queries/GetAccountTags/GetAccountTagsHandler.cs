using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

internal sealed class GetAccountTagsHandler(
    ILogger<GetAccountTagsHandler> logger,
    IAccountTagRepository accountTagRepository) : IQueryHandler<GetAccountTagsQuery, IEnumerable<AccountTag>>
{
    private readonly ILogger<GetAccountTagsHandler> _logger = logger;
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;

    public async Task<Result<IEnumerable<AccountTag>>> Handle(GetAccountTagsQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving Account Tags from database");
        var accountTags = await _accountTagRepository.GetPaginatedAsync(query.Page, query.PageSize, cancellationToken);
        _logger.LogInformation("Account Tags successfully retrived from database");

        return Result.Ok(accountTags);
    }
}
