using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.AccountTag.Requests;
using MyFinance.Contracts.AccountTag.Responses;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

public sealed class CreateAccountTagCommand(CreateAccountTagRequest request)
    : IUserRequiredRequest, ICommand<AccountTagResponse>
{
    public Guid CurrentUserId { get; set; }
    public Guid ManagementUnitId { get; init; } = request.ManagementUnitId;
    public string Tag { get; init; } = request.Tag;
    public string? Description { get; init; } = request.Description;
}