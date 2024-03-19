using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.RequestHandling;

namespace MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;

public sealed record ArchiveAccountTagCommand(Guid Id, string? ReasonToArchive)
    : UserBasedRequest, ICommand
{
}