using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit;

internal sealed class CreateBusinessUnitHandler : CommandHandler<CreateBusinessUnitCommand, BusinessUnit>
{
    private readonly ILogger<CreateBusinessUnitHandler> _logger;
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public CreateBusinessUnitHandler(
        ILogger<CreateBusinessUnitHandler> logger,
        IBusinessUnitRepository businessUnitRepository)
        => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

    public override Task<Result<BusinessUnit>> Handle(CreateBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new Business Unit with Name: {Name}", command.Name);
        var businessUnit = new BusinessUnit(command.Name, command.Description);
        _businessUnitRepository.Insert(businessUnit);
        _logger.LogInformation("{Name} Business Unit successfully created", command.Name);

        return Task.FromResult(Result.Ok(businessUnit));
    }
}
