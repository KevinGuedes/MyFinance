﻿using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;

public sealed record ArchiveAccountTagCommand(Guid Id, string? ReasonToArchive) : ICommand
{
}
