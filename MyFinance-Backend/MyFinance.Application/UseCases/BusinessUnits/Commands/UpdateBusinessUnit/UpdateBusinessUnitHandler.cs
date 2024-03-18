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

internal sealed class UpdateBusinessUnitHandler(IBusinessUnitRepository businessUnitRepository, ICurrentUserProvider currentUserProvider) 
    : ICommandHandler<UpdateBusinessUnitCommand, BusinessUnitResponse>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result<BusinessUnitResponse>> Handle(UpdateBusinessUnitCommand command,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var businessUnit = await _businessUnitRepository.GetByIdAsync(command.Id, currentUserId, cancellationToken);

        if (businessUnit is null)
        {
            var errorMessage = string.Format("Business Unit with Id {0} not found", command.Id);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        businessUnit.Update(command.Name, command.Description);
        _businessUnitRepository.Update(businessUnit);

        return Result.Ok(BusinessUnitMapper.DTR.Map(businessUnit));
    }
}