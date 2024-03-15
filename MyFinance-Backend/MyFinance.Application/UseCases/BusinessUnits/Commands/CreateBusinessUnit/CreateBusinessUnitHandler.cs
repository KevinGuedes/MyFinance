﻿using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

internal sealed class CreateBusinessUnitHandler(
    ILogger<CreateBusinessUnitHandler> logger,
    IBusinessUnitRepository businessUnitRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<CreateBusinessUnitCommand, BusinessUnit>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    private readonly ILogger<CreateBusinessUnitHandler> _logger = logger;

    public Task<Result<BusinessUnit>> Handle(CreateBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving current User");
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        _logger.LogInformation("Creating new Business Unit");
        var businessUnit = new BusinessUnit(command.Name, command.Description, currentUserId);
        _businessUnitRepository.Insert(businessUnit);
        _logger.LogInformation("Business Unit successfully created");

        return Task.FromResult(Result.Ok(businessUnit));
    }
}