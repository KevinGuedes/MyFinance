using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.BusinessUnit.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;

internal sealed class UpdateBusinessUnitHandler(IBusinessUnitRepository businessUnitRepository) 
    : ICommandHandler<UpdateBusinessUnitCommand, BusinessUnitResponse>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;

    public async Task<Result<BusinessUnitResponse>> Handle(UpdateBusinessUnitCommand command,
        CancellationToken cancellationToken)
    {
        var businessUnit = await _businessUnitRepository.GetByIdAsync(command.Id, command.CurrentUserId, cancellationToken);

        if (businessUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Business Unit with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        businessUnit.Update(command.Name, command.Description);
        _businessUnitRepository.Update(businessUnit);

        return Result.Ok(BusinessUnitMapper.DTR.Map(businessUnit));
    }
}