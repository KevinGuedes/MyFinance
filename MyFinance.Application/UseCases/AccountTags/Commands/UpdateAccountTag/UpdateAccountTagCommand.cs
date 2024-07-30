using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.AccountTag.Requests;
using MyFinance.Contracts.AccountTag.Responses;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

public sealed class UpdateAccountTagCommand(UpdateAccountTagRequest request)
    : ICommand<AccountTagResponse>
{
    public Guid Id { get; init; } = request.Id;
    public string Tag { get; init; } = request.Tag;
    public string? Description { get; init; } = request.Description;
}