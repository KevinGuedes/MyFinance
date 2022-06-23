using AutoMapper;
using MediatR;
using MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.BusinessUnits.Commands.RemoveBusinessUnitById;
using MyFinance.Application.BusinessUnits.Queries.GetBusinessUnits;
using MyFinance.Application.BusinessUnits.ViewModels;

namespace MyFinance.Application.BusinessUnits.ApiService
{
    public class BusinessUnitApiService : IBusinessUnitApiService
    {
        private protected readonly IMediator _mediator;
        private protected readonly IMapper _mapper;

        public BusinessUnitApiService(IMediator mediator, IMapper mapper)
             => (_mediator, _mapper) = (mediator, mapper);

        public async Task<BusinessUnitViewModel> CreateBusinessUnitAsync(
            BusinessUnitViewModel businessUnitViewModel,
            CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateBusinessUnitCommand>(businessUnitViewModel);
            var result = await _mediator.Send(command, cancellationToken);
            return _mapper.Map<BusinessUnitViewModel>(result);
        }

        public async Task<IEnumerable<BusinessUnitViewModel>> GetBusinessUnitsAsync(CancellationToken cancellationToken)
        {
            var query = new GetBusinessUnitsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return _mapper.Map<IEnumerable<BusinessUnitViewModel>>(result);
        }

        public async Task RemoveBusinessUnitByIdAsync(Guid businessUnitId, CancellationToken cancellationToken)
        {
            var command = new RemoveBusinessUnitByIdCommand(businessUnitId);
            await _mediator.Send(command, cancellationToken);
        }
    }
}
