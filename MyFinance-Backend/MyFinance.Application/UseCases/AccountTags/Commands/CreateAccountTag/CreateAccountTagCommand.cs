using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Contracts.AccountTag.Responses;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

public sealed record CreateAccountTagCommand(string Tag, string? Description)
    : UserBasedRequest, ICommand<AccountTagResponse>
{
}