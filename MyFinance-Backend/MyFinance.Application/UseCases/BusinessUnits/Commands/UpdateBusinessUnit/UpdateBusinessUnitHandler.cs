using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;

internal sealed class UpdateBusinessUnitHandler(
    ILogger<UpdateBusinessUnitHandler> logger,
    IBusinessUnitRepository businessUnitRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<UpdateBusinessUnitCommand, BusinessUnit>
{
    private readonly ILogger<UpdateBusinessUnitHandler> _logger = logger;
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result<BusinessUnit>> Handle(UpdateBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId} from database", command.Id);
        var businessUnit = await _businessUnitRepository.GetByIdAsync(command.Id, currentUserId, cancellationToken);

        if (businessUnit is null)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", command.Id);
            var errorMessage = string.Format("Business Unit with Id {0} not found", command.Id);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        _logger.LogInformation("Updating Business Unit with Id {BusinessUnitId}", command.Id);
        businessUnit.Update(command.Name, command.Description);
        _businessUnitRepository.Update(businessUnit);
        _logger.LogInformation("Business Unit with Id {BusinessUnitId} successfully updated", command.Id);

        return Result.Ok(businessUnit);
    }
}
