using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

public sealed record UpdateAccountTagCommand(
    Guid Id,
    string Tag,
    string? Description) : ICommand<AccountTag>
{
}
