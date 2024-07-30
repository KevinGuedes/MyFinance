using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.AccountTag.Requests;

namespace MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;

public sealed class ArchiveAccountTagCommand(ArchiveAccountTagRequest request) : ICommand
{
    public Guid Id { get; init; } = request.Id;
    public string? ReasonToArchive { get; init; } = request.ReasonToArchive;
}