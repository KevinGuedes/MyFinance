using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;

internal sealed class ArchiveBusinessUnitHandler(IBusinessUnitRepository businessUnitRepository, ICurrentUserProvider currentUserProvider) 
    : ICommandHandler<ArchiveBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result> Handle(ArchiveBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var businessUnit = await _businessUnitRepository.GetByIdAsync(command.Id, currentUserId, cancellationToken);

        if (businessUnit is null)
        {
            var errorMessage = $"Business Unit with Id {command.Id} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        businessUnit.Archive(command.ReasonToArchive);
        _businessUnitRepository.Update(businessUnit);

        return Result.Ok();
    }
}