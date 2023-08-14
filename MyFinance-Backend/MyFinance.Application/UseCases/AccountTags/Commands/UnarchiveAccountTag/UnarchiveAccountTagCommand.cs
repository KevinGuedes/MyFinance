using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UnarchiveAccountTag;

public sealed record UnarchiveAccountTagCommand(Guid Id) : ICommand
{
}
