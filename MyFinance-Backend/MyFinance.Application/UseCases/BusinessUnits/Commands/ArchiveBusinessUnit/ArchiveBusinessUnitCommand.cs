﻿using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;

public sealed record ArchiveBusinessUnitCommand(Guid Id, string? ReasonToArchive) 
    : ICommand, IUserBasedRequest
{
    public Guid CurrentUserId { get; set; }
}