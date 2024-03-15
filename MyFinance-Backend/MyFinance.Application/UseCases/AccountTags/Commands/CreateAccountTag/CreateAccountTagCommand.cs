using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

public sealed record CreateAccountTagCommand(string Tag, string? Description) : ICommand<AccountTag>
{
}