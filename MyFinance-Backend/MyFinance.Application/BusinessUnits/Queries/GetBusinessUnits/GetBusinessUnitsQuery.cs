using MediatR;
using MyFinance.Application.Interfaces;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Queries.GetBusinessUnits
{
    public class GetBusinessUnitsQuery : IRequest<IEnumerable<BusinessUnit>>, IQuery
    {
    }
}
