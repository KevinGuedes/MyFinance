using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Services.CurrentUserProvider;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

internal sealed class CreateBusinessUnitHandler(
    ILogger<CreateBusinessUnitHandler> logger,
    IBusinessUnitRepository businessUnitRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<CreateBusinessUnitCommand, BusinessUnit>
{
    private readonly ILogger<CreateBusinessUnitHandler> _logger = logger;
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result<BusinessUnit>> Handle(CreateBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving current User");
        var user = await _currentUserProvider.GetCurrentUserAsync(cancellationToken);

        _logger.LogInformation("Creating new Business Unit");
        var businessUnit = new BusinessUnit(command.Name, command.Description, user!);
        _businessUnitRepository.Insert(businessUnit);
        _logger.LogInformation("Business Unit successfully created");

        return Result.Ok(businessUnit);
    }
}
