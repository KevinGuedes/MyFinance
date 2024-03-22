using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.AccountTag.Responses;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

public sealed record CreateAccountTagCommand(string Tag, string? Description)
    : IUserRequiredRequest, ICommand<AccountTagResponse>
{
    public Guid CurrentUserId { get; set; }
}