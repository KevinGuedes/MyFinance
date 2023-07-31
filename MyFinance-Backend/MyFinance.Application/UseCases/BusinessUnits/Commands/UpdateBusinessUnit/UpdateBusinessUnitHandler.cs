﻿using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;

internal sealed class UpdateBusinessUnitHandler : ICommandHandler<UpdateBusinessUnitCommand, BusinessUnit>
{
    private readonly ILogger<UpdateBusinessUnitHandler> _logger;
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public UpdateBusinessUnitHandler(
        ILogger<UpdateBusinessUnitHandler> logger,
        IBusinessUnitRepository businessUnitRepository)
        => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

    public async Task<Result<BusinessUnit>> Handle(UpdateBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId} from database", command.Id);
        var businessUnit = await _businessUnitRepository.GetByIdAsync(command.Id, cancellationToken);

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