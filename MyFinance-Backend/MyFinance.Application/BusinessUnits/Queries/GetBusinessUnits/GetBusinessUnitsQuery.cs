using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Queries.GetBusinessUnits
{
    public sealed class GetBusinessUnitsQuery : Query<IEnumerable<BusinessUnit>>
    {
    }
}
