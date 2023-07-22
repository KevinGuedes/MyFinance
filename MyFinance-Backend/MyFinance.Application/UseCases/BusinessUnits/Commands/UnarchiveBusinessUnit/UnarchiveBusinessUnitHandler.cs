using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

public class UnarchiveBusinessUnitHandler : ICommandHandler<UnarchiveBusinessUnitCommand>
{
    private readonly ILogger<UnarchiveBusinessUnitHandler> _logger;
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public UnarchiveBusinessUnitHandler(
        ILogger<UnarchiveBusinessUnitHandler> logger,
        IBusinessUnitRepository businessUnitRepository)
        => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

    public async Task<Result> Handle(UnarchiveBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId}", command.Id);
        var businessUnit = await _businessUnitRepository.GetByIdAsync(command.Id, cancellationToken);

        if (businessUnit is null)
        {
            var errorMessage = string.Format("Business Unit with Id {0} not found", command.Id);
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", command.Id);
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
