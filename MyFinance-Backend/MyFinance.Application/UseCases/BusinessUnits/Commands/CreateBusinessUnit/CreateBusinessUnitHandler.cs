using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.BusinessUnit.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

internal sealed class CreateBusinessUnitHandler(IBusinessUnitRepository businessUnitRepository)
    : ICommandHandler<CreateBusinessUnitCommand, BusinessUnitResponse>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;

    public async Task<Result<BusinessUnitResponse>> Handle(CreateBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        var businessUnit = new BusinessUnit(command.Name, command.Description, command.CurrentUserId);
        await _businessUnitRepository.InsertAsync(businessUnit, cancellationToken);

        return Result.Ok(BusinessUnitMapper.DTR.Map(businessUnit));
    }
}