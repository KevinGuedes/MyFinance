using MyFinance.Application.Common.RequestHandling;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Queries.GetBusinessUnits
{
    public sealed class GetBusinessUnitsQuery : Query<IEnumerable<BusinessUnit>>
    {
    }
}
