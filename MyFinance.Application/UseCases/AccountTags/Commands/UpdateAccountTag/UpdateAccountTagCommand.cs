using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.AccountTag.Responses;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

public sealed record UpdateAccountTagCommand(Guid Id, string Tag, string? Description)
    : ICommand<AccountTagResponse>
{
}