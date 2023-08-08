using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

internal sealed class CreateBusinessUnitHandler : ICommandHandler<CreateBusinessUnitCommand, BusinessUnit>
{
    private readonly ILogger<CreateBusinessUnitHandler> _logger;
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public CreateBusinessUnitHandler(
        ILogger<CreateBusinessUnitHandler> logger,
        IBusinessUnitRepository businessUnitRepository)
        => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

    public Task<Result<BusinessUnit>> Handle(CreateBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new Business Unit");
        var businessUnit = new BusinessUnit(command.Name, command.Description);
        _businessUnitRepository.Insert(businessUnit);
        _logger.LogInformation("Business Unit successfully created");

        return Task.FromResult(Result.Ok(businessUnit));
    }
}
