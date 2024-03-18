using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.BusinessUnit.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

internal sealed class CreateBusinessUnitHandler(IBusinessUnitRepository businessUnitRepository, ICurrentUserProvider currentUserProvider) 
    : ICommandHandler<CreateBusinessUnitCommand, BusinessUnitResponse>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public Task<Result<BusinessUnitResponse>> Handle(CreateBusinessUnitCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var businessUnit = new BusinessUnit(command.Name, command.Description, currentUserId);
        _businessUnitRepository.Insert(businessUnit);

        return Task.FromResult(Result.Ok(BusinessUnitMapper.DTR.Map(businessUnit)));
    }
}