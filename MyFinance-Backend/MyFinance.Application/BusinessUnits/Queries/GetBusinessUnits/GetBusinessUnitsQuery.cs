using MyFinance.Application.Interfaces;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Queries.GetBusinessUnits
{
    public sealed class GetBusinessUnitsQuery : IQuery<IEnumerable<BusinessUnit>>
    {
    }
}
