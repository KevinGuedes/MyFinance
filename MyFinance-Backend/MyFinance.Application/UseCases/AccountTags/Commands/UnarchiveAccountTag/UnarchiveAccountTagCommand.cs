using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UnarchiveAccountTag;

public sealed record UnarchiveAccountTagCommand(Guid Id) : ICommand, IUserBasedRequest
{
    public Guid CurrentUserId { get; set; }
}