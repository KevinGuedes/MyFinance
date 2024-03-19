using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.RequestHandling;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UnarchiveAccountTag;

public sealed record UnarchiveAccountTagCommand(Guid Id) : UserBasedRequest, ICommand
{
}