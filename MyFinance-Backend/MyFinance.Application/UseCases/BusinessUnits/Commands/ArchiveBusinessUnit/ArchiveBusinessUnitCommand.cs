using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.RequestHandling;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;

public sealed record ArchiveBusinessUnitCommand(Guid Id, string? ReasonToArchive) 
    : UserBasedRequest, ICommand
{
}