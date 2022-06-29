using AutoMapper;
using MediatR;

namespace MyFinance.Application.Generics
{
    public abstract class EntityApiService
    {
        private protected readonly IMediator _mediator;
        private protected readonly IMapper _mapper;

        public EntityApiService(IMediator mediator, IMapper mapper)
            => (_mediator, _mapper) = (mediator, mapper);
    }
}
