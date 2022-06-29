using AutoMapper;
using MediatR;
using MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.BusinessUnits.Queries.GetBusinessUnits;
using MyFinance.Application.BusinessUnits.ViewModels;
using MyFinance.Application.Generics;

namespace MyFinance.Application.BusinessUnits.ApiService
{
    public class BusinessUnitApiService : EntityApiService, IBusinessUnitApiService
    {
        public BusinessUnitApiService(IMediator mediator, IMapper mapper)
             : base(mediator, mapper)
        {
        }

        public async Task<BusinessUnitViewModel> CreateBusinessUnitAsync(
            CreateBusinessUnitCommand command,
            CancellationToken cancellationToken)
            => _mapper.Map<BusinessUnitViewModel>(await _mediator.Send(command, cancellationToken));

        public async Task<IEnumerable<BusinessUnitViewModel>> GetBusinessUnitsAsync(CancellationToken cancellationToken)
        {
            var query = new GetBusinessUnitsQuery(); //oque vai dar se for na controller?
            var result = await _mediator.Send(query, cancellationToken);
            return _mapper.Map<IEnumerable<BusinessUnitViewModel>>(result);
        }

        public async Task<BusinessUnitViewModel> UpdateBusinessUnitAsync(UpdateBusinessUnitCommand command, CancellationToken cancellationToken)
            => _mapper.Map<BusinessUnitViewModel>(await _mediator.Send(command, cancellationToken));
    }
}
