using FluentResults;
using MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;
using MyFinance.Application.UseCases.AccountTags.DTOs;

namespace MyFinance.Application.UseCases.AccountTags.ApiService;

public interface IAccountTagApiService
{
    Task<Result<IEnumerable<AccountTagDTO>>> GetAccountTagsAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<Result<AccountTagDTO>> CreateAccountTagAsync(CreateAccountTagCommand command, CancellationToken cancellationToken);
    Task<Result<AccountTagDTO>> UpdateAccountTagAsync(UpdateAccountTagCommand command, CancellationToken cancellationToken);
    Task<Result> ArchiveAccountTagAsync(ArchiveAccountTagCommand command, CancellationToken cancellationToken);
    Task<Result> UnarchiveAccountTagAsync(Guid id, CancellationToken cancellationToken);
}
