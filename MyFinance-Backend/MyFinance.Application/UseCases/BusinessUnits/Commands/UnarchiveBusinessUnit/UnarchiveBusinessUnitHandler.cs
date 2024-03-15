using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

public class UnarchiveBusinessUnitHandler(
    ILogger<UnarchiveBusinessUnitHandler> logger,
    IBusinessUnitRepository businessUnitRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<UnarchiveBusinessUnitCommand>
{
    private readonly ILogger<UnarchiveBusinessUnitHandler> _logger = logger;
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result> Handle(UnarchiveBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId}", command.Id);
        var businessUnit = await _businessUnitRepository.GetByIdAsync(command.Id, currentUserId, cancellationToken);

        if (businessUnit is null)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", command.Id);
            var errorMessage = string.Format("Business Unit with Id {0} not found", command.Id);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        _logger.LogInformation("Unarchiving Business Unit with Id {BusinessUnitId}", command.Id);
        businessUnit.Unarchive();
        _businessUnitRepository.Update(businessUnit);
        _logger.LogInformation("Business Unit with Id {BusinessUnitId} successfully unarchived", command.Id);

        return Result.Ok();
    }
}
