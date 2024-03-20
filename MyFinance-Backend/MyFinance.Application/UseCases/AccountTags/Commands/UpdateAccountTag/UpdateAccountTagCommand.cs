using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Contracts.AccountTag.Responses;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

public sealed record UpdateAccountTagCommand(Guid Id, string Tag, string? Description)
    : UserBasedRequest, ICommand<AccountTagResponse>
{
}